using System;
using System.Collections.Generic;
using System.Text;
using TrainingTask.Common.Enums;

namespace TrainingTask.DAL.Entities
{
    public class TaskNh
    {
        public TaskNh()
        {
            Employees = new List<EmployeeNh>();
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual TimeSpan WorkHours { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime FinishDate { get; set; }

        public virtual Status Status { get; set; }

        public virtual ProjectNh ProjectNh { get; set; }

        public virtual IList<EmployeeNh> Employees { get; set; }
    }
}
