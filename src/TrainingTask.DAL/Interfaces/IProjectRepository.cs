using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.DAL.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Gets all projects.
        /// </summary>
        IEnumerable<Project> GetAll();

        /// <summary>
        /// Insert the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        int Create(CreateProject project);


        /// <summary>
        /// Updates the specified project.
        /// </summary>
        /// <param name="item">The project.</param>
        void Update(CreateProject item);
    }
}
