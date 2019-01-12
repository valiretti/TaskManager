using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using TrainingTask.Common.Enums;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.NHRepositories.Mappings
{
    public class TaskMap : ClassMap<TaskNh>
    {
        public TaskMap()
        {
            Table("Tasks");
            Id(t => t.Id);
            Map(t => t.Name);
            Map(t => t.WorkHours).Column("WorkTime");
            Map(t => t.StartDate);
            Map(t => t.FinishDate);
            Map(t => t.Status).CustomType<Status>();
            References(t => t.Project).Column("ProjectId")
                .Cascade.SaveUpdate();
            HasManyToMany(e => e.Employees)
                .ChildKeyColumn("EmployeeId")
                .ParentKeyColumn("TaskId")
                .Cascade.SaveUpdate()
                .Table("EmployeeTasks");
        }
    }
}
