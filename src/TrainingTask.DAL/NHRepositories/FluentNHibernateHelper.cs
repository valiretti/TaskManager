using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using TrainingTask.DAL.NHRepositories.Mappings;

namespace TrainingTask.DAL.NHRepositories
{
    public class FluentNHibernateHelper
    {
        public static ISession OpenSession(string connectionString)
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(connectionString).ShowSql())
                    .Mappings(m =>
                    m.FluentMappings
                    .AddFromAssemblyOf<EmployeeMap>())
                    .Mappings(m =>
                    m.FluentMappings
                    .AddFromAssemblyOf<TaskMap>())
                    .Mappings(m =>
                    m.FluentMappings
                    .AddFromAssemblyOf<ProjectMap>())
                    .BuildSessionFactory();

            return sessionFactory.OpenSession();
        }
    }
}