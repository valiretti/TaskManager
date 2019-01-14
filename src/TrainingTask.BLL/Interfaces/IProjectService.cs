using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.BLL.Interfaces
{
    public interface IProjectService : IService<Project>
    {
        /// <summary>
        /// Adds the specified project associated with tasks and employees.
        /// </summary>
        /// <param name="project">The project.</param>
        int Add(CreateProject project);

        /// <summary>
        /// Updates the specified project associated with tasks and employees.
        /// </summary>
        /// <param name="project">The project.</param>
        void Update(CreateProject project);

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Project> GetAll();
    }
}
