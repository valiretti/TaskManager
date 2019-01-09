using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.BLL.Interfaces
{
    public interface ITaskService : IService<Task>
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
        /// Gets all tasks include employees.
        /// </summary>
        IEnumerable<TaskViewModel> GetAll();

        /// <summary>
        /// Gets the task including employees by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        TaskViewModel GetViewModel(int id);

        /// <summary>
        /// Gets all tasks including employees by project identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        IEnumerable<TaskViewModel> GetByProjectId(int id);
    }
}
