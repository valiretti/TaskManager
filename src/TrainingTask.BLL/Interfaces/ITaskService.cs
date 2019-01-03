using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.BLL.Interfaces
{
    public interface ITaskService
    {
        /// <summary>
        /// Adds the specified task associated with employees.
        /// </summary>
        /// <param name="task">The task.</param>
        int Add(CreateTask task);

        /// <summary>
        /// Updates the specified task associated with employees.
        /// </summary>
        /// <param name="task">The task.</param>
        void Update(CreateTask task);

        /// <summary>
        /// Deletes the task by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);

        /// <summary>
        /// Gets the task by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task Get(int id);

        /// <summary>
        /// Gets all tasks include employees.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TaskViewModel> GetAll();

        /// <summary>
        /// Gets the task including employees by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TaskViewModel GetViewModel(int id);

        /// <summary>
        /// Gets all tasks including employees by project identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskViewModel> GetByProjectId(int id);
    }
}
