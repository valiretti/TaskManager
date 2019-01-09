using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NHibernate;
using NHibernate.Linq;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.NHRepositories
{
    public class ProjectNhRepository : IProjectRepository
    {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public ProjectNhRepository(ISession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;
        }

        public Project Get(int id)
        {
            var employee = _session.Get<ProjectNh>(id);
            return _mapper.Map<Project>(employee);
        }

        public void Delete(int id)
        {
            _session.Query<ProjectNh>()
                .Where(c => c.Id == id)
                .Delete();
        }

        public IEnumerable<Project> GetAll()
        {
            var employees = _session.Query<ProjectNh>().ToList();
            return _mapper.Map<IEnumerable<Project>>(employees);

        }

        public int Create(CreateProject project)
        {
            throw new NotImplementedException();
        }

        public void Update(CreateProject item)
        {
            throw new NotImplementedException();
        }
    }
}
