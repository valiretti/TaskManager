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
    public class TaskNhRepository : ITaskRepository
    {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public TaskNhRepository(ISession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;
        }

        public Task Get(int id)
        {
            var task = _session.Get<TaskNh>(id);
            return _mapper.Map<Task>(task);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskViewModel> GetAll()
        {
            var tasks = _session.Query<TaskNh>().Select(t => new TaskViewModel()
            {
                Id = t.Id,
                ProjectId = t.Project.Id,
                FinishDate = t.FinishDate,
                StartDate = t.StartDate,
                Status = t.Status,
                WorkHours = t.WorkHours,
                Name = t.Name,
                ProjectAbbreviation = t.Project.Abbreviation,
                EmployeeIds = t.Employees.Select(e => e.Id),
               // FullNames = t.Employees.Select(e => $"{e.FirstName} {e.LastName} {e.Patronymic}"),

            }).ToList();


            //var tasks = _session.QueryOver<TaskNh>().JoinQueryOver(c => c.Project)
            //    .SelectList(l => l
            //    .Select(c => c.Id).WithAlias(() => taskViewModel.Id)
            //    .Select(c => c.Name).WithAlias(() => taskViewModel.Name)
            //    .Select(c => c.Project.Abbreviation).WithAlias(() => taskViewModel.ProjectAbbreviation)
            //    .Select(c => c.StartDate).WithAlias(() => taskViewModel.StartDate)
            //    .Select(c => c.FinishDate).WithAlias(() => taskViewModel.FinishDate)
            //    .Select(c => c.Status).WithAlias(() => taskViewModel.Status)
            //    .Select(c => c.WorkHours).WithAlias(() => taskViewModel.WorkHours)
            //    .Select(c => c.Project.Id).WithAlias(() => taskViewModel.ProjectId))
            //    //.Select(c => c.Employees.Select(e => Projections.Concat($"{e.FirstName} {e.LastName} {e.Patronymic}"))).WithAlias(() => taskViewModel.FullNames))
            //    .TransformUsing(Transformers.AliasToBean<TaskViewModel>())
            //    .List<TaskViewModel>();

            //First firstReference = null;
            //Second secondReference = null;
            //var items = Session().QueryOver(() => firstReference)
            //    .JoinAlias(() => firstReference.Seconds, () => secondReference)
            //    .Select(Projections.Distinct(
            //        Projections.ProjectionList()
            //            .Add(Projections.Property(() => firstReference.Name).WithAlias(() => firstReference.Name))
            //            .Add(Projections.Property(() => secondReference.Year).WithAlias(() => secondReference.Year))))
            //    .TransformUsing(Transformers.AliasToBean(typeof(FooBar)))
            //    .List<FooBar>();
            return tasks;
        }

        public TaskViewModel GetViewModel(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskViewModel> GetByProjectId(int id)
        {
            throw new NotImplementedException();
        }

        public int Create(CreateTask item)
        {
            throw new NotImplementedException();
        }

        public void Update(CreateTask item)
        {
            throw new NotImplementedException();
        }
    }
}
