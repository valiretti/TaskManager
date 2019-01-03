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
    }
}
