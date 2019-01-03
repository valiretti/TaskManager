using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.BLL.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Adds the employee by specified employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns></returns>
        int Add(Employee employee);

        /// <summary>
        /// Updates the specified employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        void Update(Employee employee);

        /// <summary>
        /// Deletes the employee by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);

        /// <summary>
        /// Gets the employee by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Employee Get(int id);

        /// <summary>
        /// Gets all employees.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Employee> GetAll();
    }
}
