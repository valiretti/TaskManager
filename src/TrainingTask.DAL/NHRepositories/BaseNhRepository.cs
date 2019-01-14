using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using NHibernate;
using NHibernate.Exceptions;
using NHibernate.Linq;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Interfaces;

namespace TrainingTask.DAL.NHRepositories
{
    public abstract class BaseNhRepository<T> where T : class
    {
        protected ISession Session { get; }
        protected IMapper Mapper { get; }
        public ILog Log { get; }

        protected BaseNhRepository(ISession session, IMapper mapper, ILog log)
        {
            Session = session;
            Mapper = mapper;
            Log = log;
        }

        public void Delete(int id)
        {
            var entity = Session.Get<T>(id);
            Session.Delete(entity);
            Session.Flush();
        }

        protected TResult Get<TResult>(int id)
        {
            Log.Trace($"Requested Id: {id} ");
            Log.Trace("Attempt to connect to data source");

            var entity = Session.Get<T>(id);

            Log.Trace("The selection was successful.");

            return Mapper.Map<TResult>(entity);
        }

        protected IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] fetches)
        {
            var entities = Session.Query<T>();
            if (fetches != null)
            {
                foreach (var fetch in fetches)
                {
                    entities = entities.Fetch(fetch);
                }
            }
            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            Log.Debug($"Get from database {entities.Count()} items");
            return Mapper.Map<IEnumerable<TResult>>(entities);
        }

        protected int Create<TEntity>(TEntity item)
        {
            try
            {
                var entity = Mapper.Map<T>(item);
                var id = Session.Save(entity);
                Session.Flush();
                return (int)id;
            }
            catch (GenericADOException ex) when (IsForeignKeyViolation(ex))
            {
                var message = ForeignKeyViolationLog(ex);
                throw new ForeignKeyViolationException(message, ex);

            }
            catch (GenericADOException ex) when (IsUniquenessViolation(ex))
            {
                var message = UniquenessViolationLog(ex);
                throw new UniquenessViolationException(message, ex);
            }
        }

        protected void Update<TEntity>(int id, TEntity item)
        {
            try
            {
                var entity = Session.Get<T>(id);
                Mapper.Map(item, entity);
                Session.Update(entity);
                Session.Flush();
            }
            catch (GenericADOException ex) when (IsForeignKeyViolation(ex))
            {
                var message = ForeignKeyViolationLog(ex);
                throw new ForeignKeyViolationException(message, ex);

            }
            catch (GenericADOException ex) when (IsUniquenessViolation(ex))
            {
                var message = UniquenessViolationLog(ex);
                throw new UniquenessViolationException(message, ex);
            }
        }

        private string UniquenessViolationLog(GenericADOException ex)
        {
            var message =
                $"The project already exists.";
            Log.Error($"{message} SqlException message : {ex.Message}");
            return message;
        }

        private static bool IsUniquenessViolation(GenericADOException ex)
        {
            return ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627);
        }

        private string ForeignKeyViolationLog(GenericADOException ex)
        {
            var message =
                $"The item has already deleted.";
            Log.Error($"{message} SqlException message : {ex.Message}");
            return message;
        }

        private static bool IsForeignKeyViolation(GenericADOException ex)
        {
            return ex.InnerException is SqlException sqlEx && (sqlEx.Number == 547 || sqlEx.Number == 515);
        }
    }
}
