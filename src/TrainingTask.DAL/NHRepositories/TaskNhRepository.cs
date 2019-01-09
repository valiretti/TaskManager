using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.NHRepositories
{
    public class TaskNhRepository : BaseNhRepository<TaskNh>, ITaskRepository
    {
        public TaskNhRepository(ISession session, IMapper mapper) : base(session, mapper)
        {
        }

        public Task Get(int id) => Get<Task>(id);

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskViewModel> GetAll() => GetAll<TaskViewModel>(t => t.Project, t => t.Employees);

        public TaskViewModel GetViewModel(int id) => Get<TaskViewModel>(id);

        public IEnumerable<TaskViewModel> GetByProjectId(int id)
        {
            throw new NotImplementedException();
        }

        public int Create(CreateTask item) => Create<CreateTask>(item);

        public void Update(CreateTask item) => Update(item.Id, item);
    }
}
