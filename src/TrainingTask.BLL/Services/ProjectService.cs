using System.Collections.Generic;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.BLL.Services
{
    public class ProjectService : Service<Project>, IProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public int Add(CreateProject project)
        {
            return _repository.Create(project);
        }

        public void Update(CreateProject project)
        {
            _repository.Update(project);
        }

        public IEnumerable<Project> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
