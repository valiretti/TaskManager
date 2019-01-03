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
    }
}
