using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace TrainingTask.DAL.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        private readonly ILog _log;
        private readonly ITaskRepository _taskRepository;

        public ProjectRepository(string connectionString, ILog log, ITaskRepository taskRepository) : base(connectionString)
        {
            _log = log;
            _taskRepository = taskRepository;
        }

        public Project Get(int id)
        {
            return base.GetAll(
                $"SELECT TOP 1 Id, Name, Abbreviation, Description FROM Projects WHERE Id = {id}",
                record => new Project()
                {
                    Id = record.GetInt32(0),
                    Name = record.GetString(1),
                    Abbreviation = record.GetString(2),
                    Description = record.GetString(3)
                }
            ).FirstOrDefault();
        }

        public int Create(CreateProject project)
        {
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    var insertedId = base.Create(
                        $@"INSERT INTO Projects (Name, Abbreviation, Description) 
                   VALUES ({project.Name}, {project.Abbreviation}, {project.Description}) 
                   SET @id=SCOPE_IDENTITY()");

                    if (project.Tasks != null)
                    {
                        foreach (var projectTask in project.Tasks)
                        {
                            projectTask.ProjectId = insertedId;
                            _taskRepository.Create(projectTask);
                        }
                    }

                    transactionScope.Complete();
                    return insertedId;
                }
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                var message =
                    $"The project with the name {project.Name} and abbreviation {project.Abbreviation} already exists.";
                _log.Error($"{message} SqlException message : {ex.Message}");
                throw new UniquenessViolationException(message, ex);
            }
        }

        public void Update(CreateProject item)
        {
            var tasks = _taskRepository.GetByProjectId(item.Id).Where(m => item.Tasks.All(t => t.Id != m.Id)).ToArray();
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    base.Update(
                        $@"UPDATE Projects SET 
                   Name = {item.Name}, Abbreviation = {item.Abbreviation}, Description = {item.Description} WHERE Id = {item.Id}");

                    foreach (var projectTask in item.Tasks)
                    {
                        projectTask.ProjectId = item.Id;
                        if (projectTask.Id == 0)
                        {
                            _taskRepository.Create(projectTask);
                        }
                        else
                        {
                            _taskRepository.Update(projectTask);
                        }
                    }
                    foreach (var task in tasks)
                    {
                        _taskRepository.Delete(task.Id);
                    }

                    transactionScope.Complete();
                }
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                var message =
                    $"The project with the name {item.Name} or abbreviation {item.Abbreviation} already exists.";
                _log.Error($"{message} SqlException message : {ex.Message}");
                throw new UniquenessViolationException(message, ex);
            }
        }

        public void Delete(int id)
        {
            base.Delete($"DELETE FROM Projects WHERE Id = {id}");
        }

        public IEnumerable<Project> GetAll()
        {
            return base.GetAll<Project>(
                "SELECT Id, Name, Abbreviation, Description FROM Projects",
                null,
                record => new Project()
                {
                    Id = record.GetInt32(0),
                    Name = record.GetString(1),
                    Abbreviation = record.GetString(2),
                    Description = record.GetString(3)
                });
        }
    }
}

