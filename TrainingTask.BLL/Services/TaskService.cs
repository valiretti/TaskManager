using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;

        public TaskService(ITaskRepository repository, IEmployeeTaskRepository employeeTaskRepository)
        {
            _repository = repository;
            _employeeTaskRepository = employeeTaskRepository;
        }

        public int Add(CreateTask task)
        {
            var nTask = new Task
            {
                Name = task.Name,
                WorkHours = task.WorkHours,
                StartDate = task.StartDate,
                FinishDate = task.FinishDate,
                Status = task.Status,
                ProjectId = task.ProjectId
            };

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
                var insertedId = _repository.Create(nTask);
                if (task.Employees != null)
                {
                    _employeeTaskRepository.DeleteEmployeesFromTask(insertedId);
                    foreach (var employee in task.Employees)
                    {
                        _employeeTaskRepository.Add(insertedId, employee.Id);
                    }
                }
                transactionScope.Complete();
                return insertedId;
            }
        }

        public void Update(CreateTask modifiedTask)
        {
            var task = _repository.Get(modifiedTask.Id);

            task.Name = modifiedTask.Name;
            task.WorkHours = modifiedTask.WorkHours;
            task.Status = modifiedTask.Status;
            task.StartDate = modifiedTask.StartDate;
            task.FinishDate = modifiedTask.FinishDate;
            task.ProjectId = modifiedTask.ProjectId;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
                _repository.Update(task);
                if (modifiedTask.Employees != null)
                {
                    _employeeTaskRepository.DeleteEmployeesFromTask(task.Id);
                    foreach (var employee in modifiedTask.Employees)
                    {
                        _employeeTaskRepository.Add(task.Id, employee.Id);
                    }
                }

                transactionScope.Complete();
            }
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public Task Get(int id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<TaskViewModel> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
