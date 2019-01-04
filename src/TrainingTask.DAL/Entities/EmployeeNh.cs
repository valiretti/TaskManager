using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingTask.DAL.Entities
{
    public class EmployeeNh
    {
        public EmployeeNh()
        {
            Tasks = new List<TaskNh>();
        }

        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Patronymic { get; set; }

        public string Position { get; set; }

        public virtual IList<TaskNh> Tasks { get; set; }
    }
}
