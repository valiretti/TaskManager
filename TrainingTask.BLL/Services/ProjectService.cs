using System.Collections.Generic;
using System.Transactions;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskService _taskService;

        public ProjectService(IProjectRepository projectRepository, ITaskService taskService)
        {
            _projectRepository = projectRepository;
            _taskService = taskService;
        }

        public int Add(CreateProject project)
        {
            var nProject = new Project
            {
                Name = project.Name,
                Abbreviation = project.Abbreviation,
                Description = project.Description
            };

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
                var insertedId = _projectRepository.Create(nProject);

                if (project.Tasks != null)
                {
                    foreach (var projectTask in project.Tasks)
                    {
                        _taskService.Add(projectTask);
                    }
                }

                transactionScope.Complete();
                return insertedId;
            }
        }

        public void Update(CreateProject modifiedProject)
        {
            var project = _projectRepository.Get(modifiedProject.Id);
            project.Name = modifiedProject.Name;
            project.Abbreviation = modifiedProject.Abbreviation;
            project.Description = modifiedProject.Description;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
                _projectRepository.Update(project);
                foreach (var projectTask in modifiedProject.Tasks)
                {
                    _taskService.Update(projectTask);
                }

                transactionScope.Complete();
            }
        }

        public void Delete(int id)
        {
            _projectRepository.Delete(id);
        }

        public Project Get(int id)
        {
            return _projectRepository.Get(id);
        }

        public IEnumerable<Project> GetAll()
        {
            return _projectRepository.GetAll();
        }
    }
}
