using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using NHibernate;
using NHibernate.Linq;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.NHRepositories
{
    public abstract class BaseNhRepository<T> where T : class 
    {
        protected ISession Session { get; }
        protected IMapper Mapper { get; }

        protected BaseNhRepository(ISession session, IMapper mapper)
        {
            Session = session;
            Mapper = mapper;
        }

        protected TResult Get<TResult>(int id)
        {
            var entity = Session.Get<T>(id);
            return Mapper.Map<TResult>(entity);
        }

        protected IEnumerable<TResult> GetAll<TResult>(params Expression<Func<T, object>>[] fetches)
        {
            var entities = Session.Query<T>();
            if (fetches != null)
            {
                foreach (var fetch in fetches)
                {
                    entities = entities.Fetch(fetch);
                }
            }
            return Mapper.Map<IEnumerable<TResult>>(entities);
        }

        protected int Create<TEntity>(TEntity item)
        {
            var employee = Mapper.Map<T>(item);
            var id = Session.Save(employee);
            return (int)id;
        }

        protected void Update<TEntity>(int id, TEntity item)
        {
            var employee = Session.Get<T>(id);
            Mapper.Map(item, employee);
            Session.Update(employee);
            Session.Flush();
        }
    }
}
