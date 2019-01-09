using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.DAL.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        /// <summary>
        /// Gets all employees.
        /// </summary>
        IEnumerable<Employee> GetAll();

        /// <summary>
        /// Insert the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
         int Create(Employee item);

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(Employee item);
    }
}
