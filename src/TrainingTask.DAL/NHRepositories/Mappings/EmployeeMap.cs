using FluentNHibernate.Mapping;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.NHRepositories.Mappings
{
    public class EmployeeMap : ClassMap<EmployeeNh>
    {
        public EmployeeMap()
        {
            Id(e => e.Id);
            Map(e => e.FirstName);
            Map(e => e.LastName);
            Map(e => e.Patronymic);
            Map(e => e.Position);
            HasManyToMany(e => e.Tasks)
                .Cascade.All()
                .Inverse()
                .Table("EmployeeTasks");
        }
    }
}
