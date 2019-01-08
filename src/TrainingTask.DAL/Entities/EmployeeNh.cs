using System.Collections.Generic;

namespace TrainingTask.DAL.Entities
{
    public class EmployeeNh : BaseEntity
    {
        public EmployeeNh()
        {
            Tasks = new HashSet<TaskNh>();
        }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Patronymic { get; set; }

        public string Position { get; set; }

        public virtual ISet<TaskNh> Tasks { get; set; }
    }
}
