using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using TrainingTask.DAL.Profiles;

namespace TrainingTask.Web
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg => {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c =>
                {
                    var ctx = c.Resolve<IComponentContext>();
                    return c.Resolve<MapperConfiguration>().CreateMapper(ctx.Resolve);
                })
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}
