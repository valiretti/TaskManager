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

        public AdoNetModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var parameter = new NamedParameter("connectionString", _connectionString);

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsAssignableTo<BaseRepository>() && !t.IsAbstract)
                .WithParameter(parameter)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
