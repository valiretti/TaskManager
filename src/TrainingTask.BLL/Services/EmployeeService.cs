using System.Collections.Generic;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public int Add(Employee employee)
        {
            return _repository.Create(employee);
        }

        public void Update(Employee employee)
        {
            if (employee != null)
            {
                _repository.Update(employee);
            }
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public Employee Get(int id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
