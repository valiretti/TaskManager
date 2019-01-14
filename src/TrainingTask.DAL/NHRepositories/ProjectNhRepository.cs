using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NHibernate;
using NHibernate.Linq;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.NHRepositories
{
    public class ProjectNhRepository : BaseNhRepository<ProjectNh>, IProjectRepository
    {
        public ProjectNhRepository(ISession session, IMapper mapper, ILog log) : base(session, mapper, log)
        {
        }

        public Project Get(int id) => Get<Project>(id);
     
        public IEnumerable<Project> GetAll() => GetAll<Project>();

        public int Create(CreateProject project) => Create<CreateProject>(project);

        public void Update(CreateProject item) => Update(item.Id, item);

    }
}
