using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TrainingTask.Common.Enums;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
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

        public int Create(Task item)
        {
            return base.Create(
                $@"INSERT INTO Tasks (Name, WorkTime, StartDate, FinishDate, Status, ProjectId) 
                   VALUES ({item.Name}, {item.WorkHours.TotalMinutes}, {item.StartDate}, {item.FinishDate}, {(int)item.Status}, {item.ProjectId} ) 
                   SET @id=SCOPE_IDENTITY()");
        }

        public void Update(Task item)
        {
            base.Update(
                $@"UPDATE Tasks SET 
                    Name = {item.Name}, WorkTime = {item.WorkHours.TotalMinutes}, StartDate = {item.StartDate}, FinishDate = {item.FinishDate}, 
                    Status = {(int)item.Status}, ProjectId = {item.ProjectId} WHERE Id = {item.Id}");
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
               record =>
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
                       Status = (Status)record.GetInt32(8),
                       ProjectId = record.GetInt32(9),
                       EmployeeId = employeeId,
                       WorkHours = TimeSpan.FromMinutes(record.GetInt32(11))
                   };
               });

            var taskGroups = tasks
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

            return taskGroups.FirstOrDefault();
        }

        public IEnumerable<TaskViewModel> GetByProjectId(int id)
        {
            var tasks = base.GetAll(
               $@"SELECT Tasks.Id, Projects.Abbreviation, Tasks.Name, Tasks.StartDate, Tasks.FinishDate, Employees.FirstName, Employees.LastName, Employees.Patronymic, Tasks.Status, Tasks.ProjectId, Employees.Id, Tasks.WorkTime 
                    FROM Tasks JOIN Projects ON Projects.Id = Tasks.ProjectId 
                    LEFT JOIN EmployeeTasks ON EmployeeTasks.TaskId = Tasks.Id 
                    LEFT JOIN Employees ON Employees.Id = EmployeeTasks.EmployeeId 
                    WHERE Tasks.ProjectId = {id}",
               record =>
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
                       Status = (Status)record.GetInt32(8),
                       ProjectId = record.GetInt32(9),
                       EmployeeId = employeeId,
                       WorkHours = TimeSpan.FromMinutes(record.GetInt32(11))
                   };
               });

            var taskGroups = tasks
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

            return taskGroups;
        }

        private class TaskModel
        {
            public int Id { get; set; }

            public string ProjectAbbreviation { get; set; }

            public string Name { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime FinishDate { get; set; }

            public string FullName { get; set; }

            public Status Status { get; set; }

            public int ProjectId { get; set; }

            public int? EmployeeId { get; set; }

            public TimeSpan WorkHours { get; set; }

        }
    }
}
