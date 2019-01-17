using System.Collections.Generic;
using TrainingTask.BLL.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.BLL.Services
{
    public class EmployeeService : Service<Employee>, IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public int Add(Employee employee)
        {
            return _repository.Create(employee);
        }

        public void Update(Employee employee)
        {
            _repository.Update(employee);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _repository.GetAll();
        }

        public Page<Employee> Get(int pageIndex, int limit)
        {
            return _repository.Get(pageIndex, limit);
        }
    }
}
