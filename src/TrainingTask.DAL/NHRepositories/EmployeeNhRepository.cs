using System.Collections.Generic;
using AutoMapper;
using NHibernate;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.NHRepositories
{
    public class EmployeeNhRepository : BaseNhRepository<EmployeeNh>, IEmployeeRepository
    {
        public EmployeeNhRepository(ISession session, IMapper mapper, ILog log) : base(session, mapper, log)
        {
        }

        public Employee Get(int id) => Get<Employee>(id);

        public IEnumerable<Employee> GetAll() => GetAll<Employee>();

        public int Create(Employee item) => Create<Employee>(item);

        public void Update(Employee item) => Update(item.Id, item);

        public Page<Employee> Get(int pageIndex, int limit) => Get<Employee>(pageIndex, limit);

    }
}
