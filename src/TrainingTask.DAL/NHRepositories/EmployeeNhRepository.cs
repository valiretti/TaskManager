using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NHibernate;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.NHRepositories
{
    public class EmployeeNhRepository : IEmployeeRepository
    {
        private readonly ISession _session;

        public EmployeeNhRepository(ISession session)
        {
            _session = session;
        }

        public Employee Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Create(Employee item)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Employee, EmployeeNh>()).CreateMapper();
            var employee = mapper.Map<Employee, EmployeeNh>(item);
            var id = _session.Save(employee);
            return (int)id;
        }

        public void Update(Employee item)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Employee, EmployeeNh>()).CreateMapper();
            var employee = mapper.Map<Employee, EmployeeNh>(item);
           _session.Update(employee);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = _session.Query<EmployeeNh>().ToList();
            var mapper = new MapperConfiguration(c => c.CreateMap<EmployeeNh, Employee>()).CreateMapper();
            return mapper.Map<IEnumerable<EmployeeNh>, IEnumerable<Employee>>(employees);
        }
    }
}
