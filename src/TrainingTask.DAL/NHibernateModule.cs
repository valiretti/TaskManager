using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using TrainingTask.DAL.NHRepositories.Mappings;

namespace TrainingTask.DAL
{
    public class NHibernateModule : Module
    {
        private readonly string _connectionString;

        public NHibernateModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ISessionFactory>(c => Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(_connectionString).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TaskMap>())
               .BuildSessionFactory()).SingleInstance();

            builder.Register<ISession>(c => c.Resolve<ISessionFactory>().OpenSession()).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("NhRepository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
