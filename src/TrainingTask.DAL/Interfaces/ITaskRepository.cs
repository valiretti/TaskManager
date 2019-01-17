using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.DAL.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
        /// <summary>
        /// Gets all tasks include employees.
        /// </summary>
        IEnumerable<TaskViewModel> GetAll();


        /// <summary>
        /// Gets the task including employees by Id.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        TaskViewModel GetViewModel(int id);


        /// <summary>
        /// Gets all tasks including employees by project identifier.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        IEnumerable<TaskViewModel> GetByProjectId(int id);

        /// <summary>
        /// Insert the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        int Create(CreateTask item);


        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(CreateTask item);

        /// <summary>
        /// Gets tasks for 1 page.
        /// </summary>
        Page<TaskViewModel> Get(int pageIndex, int limit);
    }
}
