using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NHibernate;
using NHibernate.Linq;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.NHRepositories
{
    public class EmployeeNhRepository : IEmployeeRepository
    {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public EmployeeNhRepository(ISession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;
        }

        public Employee Get(int id)
        {
            var employee = _session.Get<EmployeeNh>(id);
            return _mapper.Map<Employee>(employee);
        }

        public int Create(Employee item)
        {
            var employee = _mapper.Map<EmployeeNh>(item);
            var id = _session.Save(employee);
            return (int)id;
        }

        public void Update(Employee item)
        {
            var employee = _session.Get<EmployeeNh>(item.Id);
            _mapper.Map(item, employee);
            _session.Update(employee);
            _session.Flush();
        }

        public void Delete(int id)
        {
            _session.Query<EmployeeNh>()
                .Where(c => c.Id == id)
                .Delete();
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = _session.Query<EmployeeNh>().ToList();
            return _mapper.Map<IEnumerable<Employee>>(employees);
        }
    }
}
