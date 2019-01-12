using FluentNHibernate.Mapping;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.NHRepositories.Mappings
{
    public class EmployeeMap : ClassMap<EmployeeNh>
    {
        public EmployeeMap()
        {
            Table("Employees");
            Id(e => e.Id);
            Map(e => e.FirstName);
            Map(e => e.LastName);
            Map(e => e.Patronymic);
            Map(e => e.Position);
            HasManyToMany(e => e.Tasks)
                .ChildKeyColumn("EmployeeId")
                .ParentKeyColumn("TaskId")
                .Cascade.All()
                .Inverse()
                .Table("EmployeeTasks");
        }
    }
}
