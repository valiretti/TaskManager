using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using AutoMapper;
using TrainingTask.Common.Enums;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace TrainingTask.DAL.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        private readonly ILog _log;
        private readonly IMapper _mapper;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;

        public TaskRepository(string connectionString, ILog log, IMapper mapper, IEmployeeTaskRepository employeeTaskRepository) : base(connectionString)
        {
            _log = log;
            _mapper = mapper;
            _employeeTaskRepository = employeeTaskRepository;
        }

        public Task Get(int id)
        {
            return base.GetAll(
                $"SELECT TOP 1 Id, Name, WorkTime, StartDate, FinishDate, Status, ProjectId FROM Tasks WHERE Id = {id}",
                record => new Task()
                {
                    Id = record.GetInt32(0),
                    Name = record.GetString(1),
                    WorkHours = TimeSpan.FromMinutes(record.GetInt32(2)),
                    StartDate = record.GetDateTime(3),
                    FinishDate = record.GetDateTime(4),
                    Status = (Status)record.GetInt32(5),
                    ProjectId = record.GetInt32(6)
                }
            ).FirstOrDefault();
        }

        public int Create(CreateTask task)
        {
            var item = _mapper.Map<Task>(task);
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    var insertedId = base.Create(
                        $@"INSERT INTO Tasks (Name, WorkTime, StartDate, FinishDate, Status, ProjectId) 
                   VALUES ({item.Name}, {item.WorkHours.TotalMinutes}, {item.StartDate}, {item.FinishDate}, {(int)item.Status}, {item.ProjectId} ) 
                   SET @id=SCOPE_IDENTITY()");

                    UpdateEmployees(task, insertedId);
                    transactionScope.Complete();

                    return insertedId;
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                var message =
                    $"The project with the Id {item.ProjectId} has already deleted.";
                _log.Error($"{message} SqlException message : {ex.Message}");
                throw new ForeignKeyViolationException(message, ex);
            }
        }

        public void Update(CreateTask task)
        {
            var item = _mapper.Map<Task>(task);
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    base.Update(
                        $@"UPDATE Tasks SET 
                    Name = {item.Name}, WorkTime = {item.WorkHours.TotalMinutes}, StartDate = {item.StartDate}, FinishDate = {item.FinishDate}, 
                    Status = {(int)item.Status}, ProjectId = {item.ProjectId} WHERE Id = {item.Id}");

                    UpdateEmployees(task, task.Id);
                    transactionScope.Complete();
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                var message =
                    $"The project with the Id {item.ProjectId} has already deleted.";
                _log.Error($"{message} SqlException message : {ex.Message}");
                throw new ForeignKeyViolationException(message, ex);
            }
        }

        public void Delete(int id)
        {
            base.Delete($"DELETE FROM Tasks WHERE Id = {id}");
        }

        public IEnumerable<TaskViewModel> GetAll()
        {
            var tasks = base.GetAll(
                "SELECT Tasks.Id, Projects.Abbreviation, Tasks.Name, Tasks.StartDate, Tasks.FinishDate, Employees.FirstName, Employees.LastName, Employees.Patronymic, Tasks.Status FROM Tasks " +
                "JOIN Projects ON Projects.Id = Tasks.ProjectId " +
                "LEFT JOIN EmployeeTasks ON EmployeeTasks.TaskId = Tasks.Id " +
                "LEFT JOIN Employees ON Employees.Id = EmployeeTasks.EmployeeId",
                null,
                record =>
                {
                    string firstName = record.IsDBNull(5) ? String.Empty : record.GetString(5);
                    string lastName = record.IsDBNull(6) ? String.Empty : (record.GetString(6));
                    string patronymic = record.IsDBNull(7) ? String.Empty : record.GetString(7);

                    return new TaskModel()
                    {
                        Id = record.GetInt32(0),
                        ProjectAbbreviation = record.GetString(1),
                        Name = record.GetString(2),
                        StartDate = record.GetDateTime(3),
                        FinishDate = record.GetDateTime(4),
                        FullName = $"{firstName} {lastName} {patronymic}",
                        Status = (Status)record.GetInt32(8)
                    };
                });

            var taskGroups = tasks.GroupBy(t => new { t.Id, t.Name, t.Status, t.ProjectAbbreviation, t.FinishDate, t.StartDate })
                .Select(m => new TaskViewModel
                {
                    Id = m.Key.Id,
                    ProjectAbbreviation = m.Key.ProjectAbbreviation,
                    Name = m.Key.Name,
                    StartDate = m.Key.StartDate,
                    FinishDate = m.Key.FinishDate,
                    FullNames = m.Where(p => !string.IsNullOrWhiteSpace(p.FullName)).Select(p => p.FullName),
                    Status = m.Key.Status
                });

            return taskGroups;
        }

        public TaskViewModel GetViewModel(int id)
        {
            var tasks = base.GetAll(
               $@"SELECT Tasks.Id, Projects.Abbreviation, Tasks.Name, Tasks.StartDate, Tasks.FinishDate, Employees.FirstName, Employees.LastName, Employees.Patronymic, Tasks.Status, Tasks.ProjectId, Employees.Id, Tasks.WorkTime 
                    FROM Tasks JOIN Projects ON Projects.Id = Tasks.ProjectId 
                    LEFT JOIN EmployeeTasks ON EmployeeTasks.TaskId = Tasks.Id 
                    LEFT JOIN Employees ON Employees.Id = EmployeeTasks.EmployeeId 
                    WHERE Tasks.Id = {id}",
               GetTaskModel);

            return GetTaskViewModels(tasks).FirstOrDefault();
        }

        public IEnumerable<TaskViewModel> GetByProjectId(int id)
        {
            var tasks = base.GetAll(
               $@"SELECT Tasks.Id, Projects.Abbreviation, Tasks.Name, Tasks.StartDate, Tasks.FinishDate, Employees.FirstName, Employees.LastName, Employees.Patronymic, Tasks.Status, Tasks.ProjectId, Employees.Id, Tasks.WorkTime 
                    FROM Tasks JOIN Projects ON Projects.Id = Tasks.ProjectId 
                    LEFT JOIN EmployeeTasks ON EmployeeTasks.TaskId = Tasks.Id 
                    LEFT JOIN Employees ON Employees.Id = EmployeeTasks.EmployeeId 
                    WHERE Tasks.ProjectId = {id}",
               GetTaskModel);

            return GetTaskViewModels(tasks);
        }

        private void UpdateEmployees(CreateTask task, int id)
        {
            _employeeTaskRepository.DeleteEmployeesFromTask(id);

            if (task.Employees != null)
            {
                foreach (var employee in task.Employees)
                {
                    _employeeTaskRepository.Add(employee, id);
                }
            }
        }

        private static TaskModel GetTaskModel(IDataRecord record)
        {
            string firstName = record.IsDBNull(5) ? String.Empty : record.GetString(5);
            string lastName = record.IsDBNull(6) ? String.Empty : (record.GetString(6));
            string patronymic = record.IsDBNull(7) ? String.Empty : record.GetString(7);
            int? employeeId = record.IsDBNull(10) ? default(int?) : record.GetInt32(10);

            return new TaskModel()
            {
                Id = record.GetInt32(0),
                ProjectAbbreviation = record.GetString(1),
                Name = record.GetString(2),
                StartDate = record.GetDateTime(3),
                FinishDate = record.GetDateTime(4),
                FullName = $"{firstName} {lastName} {patronymic}",
                Status = (Status) record.GetInt32(8),
                ProjectId = record.GetInt32(9),
                EmployeeId = employeeId,
                WorkHours = TimeSpan.FromMinutes(record.GetInt32(11))
            };
        }
        private static IEnumerable<TaskViewModel> GetTaskViewModels(IEnumerable<TaskModel> tasks)
        {
            return tasks
                .GroupBy(t => new { t.Id, t.Name, t.Status, t.ProjectAbbreviation, t.FinishDate, t.StartDate, t.ProjectId, t.WorkHours })
                .Select(m => new TaskViewModel
                {
                    Id = m.Key.Id,
                    ProjectAbbreviation = m.Key.ProjectAbbreviation,
                    Name = m.Key.Name,
                    StartDate = m.Key.StartDate,
                    FinishDate = m.Key.FinishDate,
                    FullNames = m.Where(p => p.EmployeeId != null).Select(p => p.FullName),
                    EmployeeIds = m.Where(p => p.EmployeeId != null).Select(p => p.EmployeeId.Value),
                    Status = m.Key.Status,
                    ProjectId = m.Key.ProjectId,
                    WorkHours = m.Key.WorkHours
                });
        }
    }
}
