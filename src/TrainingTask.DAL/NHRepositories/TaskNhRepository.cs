using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.NHRepositories
{
    public class TaskNhRepository : BaseNhRepository<TaskNh>, ITaskRepository
    {
        public TaskNhRepository(ISession session, IMapper mapper, ILog log) : base(session, mapper, log)
        {
        }

        public Task Get(int id) => Get<Task>(id);

        public IEnumerable<TaskViewModel> GetByProjectId(int id) => GetAll<TaskViewModel>(t => t.Project.Id == id);
      
        public IEnumerable<TaskViewModel> GetAll() => GetAll<TaskViewModel>(null, t => t.Project, t => t.Employees);

        public TaskViewModel GetViewModel(int id) => Get<TaskViewModel>(id);

        public int Create(CreateTask item) => Create<CreateTask>(item);

        public void Update(CreateTask item) => Update(item.Id, item);
    }
}
