using System.Collections.Generic;
using System.Linq;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.BLL.Services
{
    public class TaskService : Service<Task>, ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public int Add(CreateTask task)
        {
            return _repository.Create(task);
        }

        public void Update(CreateTask task)
        {
            _repository.Update(task);
        }

        public TaskViewModel GetViewModel(int id)
        {
            return _repository.GetViewModel(id);
        }

        public IEnumerable<TaskViewModel> GetByProjectId(int id)
        {
            return _repository.GetByProjectId(id);
        }

        public IEnumerable<TaskViewModel> GetAll()
        {
            return _repository.GetAll(); 
        }
    }
}
