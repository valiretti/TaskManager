using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.NHRepositories.Mappings
{
    public class TaskMap : ClassMap<TaskNh>
    {
        public TaskMap()
        {
            Id(t => t.Id);
            Map(t => t.Name);
            Map(t => t.WorkHours);
            Map(t => t.StartDate);
            Map(t => t.FinishDate);
            Map(t => t.Status);
            References(t => t.ProjectNh).Column("ProjectId")
                .Cascade.All();
            HasManyToMany(e => e.Employees)
                .Cascade.All()
                .Table("EmployeeTasks");
        }
    }
}
