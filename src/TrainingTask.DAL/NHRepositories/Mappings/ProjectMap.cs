using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.NHRepositories.Mappings
{
    public class ProjectMap : ClassMap<ProjectNh>
    {
        public ProjectMap()
        {
            Table("Projects");
            Id(p => p.Id);
            Map(p => p.Name);
            Map(p => p.Abbreviation);
            Map(p => p.Description);
            HasMany(p => p.Tasks)
                .Inverse()
                .Cascade.All();
        }
    }
}
