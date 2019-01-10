using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using TrainingTask.DAL.NHRepositories.Mappings;
using TrainingTask.DAL.NHRepositories.Resolvers;

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
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EmployeeMap>())
               .BuildSessionFactory()).SingleInstance();

            builder.Register<ISession>(c => c.Resolve<ISessionFactory>().OpenSession()).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("NhRepository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EntityResolver<,,,>)).AsSelf().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(CollectionResolver<,,,>)).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType(typeof(ProjectResolver)).AsSelf().InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
