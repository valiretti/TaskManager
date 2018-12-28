using System;
using System.Collections.Generic;
using System.Text;
using TrainingTask.Common.Models;

namespace TrainingTask.BLL.Interfaces
{
    public interface IEmployeeService
    {
        int Add(Employee employee);

        void Update(Employee employee);

        void Delete(int id);

        Employee Get(int id);

        IEnumerable<Employee> GetAll();
    }
}
