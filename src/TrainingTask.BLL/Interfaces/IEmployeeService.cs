using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.BLL.Interfaces
{
    public interface IEmployeeService: IService<Employee>
    {
        /// <summary>
        /// Adds the employee by specified employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        int Add(Employee employee);

        /// <summary>
        /// Updates the specified employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        void Update(Employee employee);

        /// <summary>
        /// Gets all employees.
        /// </summary>
        IEnumerable<Employee> GetAll();

        /// <summary>
        /// Gets employees for 1 page.
        /// </summary>
        Page<Employee> Get(int pageIndex, int limit);
    }
}
