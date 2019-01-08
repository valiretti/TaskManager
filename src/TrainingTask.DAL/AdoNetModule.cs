using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Core;
using TrainingTask.Common.Interfaces;
using TrainingTask.DAL.Interfaces;
using TrainingTask.DAL.Repositories;

namespace TrainingTask.DAL
{
    public class AdoNetModule : Module
    {
        private readonly string _connectionString;
        private readonly ILog _log;

        public AdoNetModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var parameter = new NamedParameter("connectionString", _connectionString);

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .WithParameter(parameter)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>()
                .WithParameter(parameter).InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
