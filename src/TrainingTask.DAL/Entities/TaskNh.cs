using System;
using System.Collections.Generic;
using TrainingTask.Common.Enums;

namespace TrainingTask.DAL.Entities
{
    public class TaskNh : BaseEntity
    {
        public TaskNh()
        {
            Employees = new HashSet<EmployeeNh>();
        }

        public virtual string Name { get; set; }

        public virtual TimeSpan WorkHours { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime FinishDate { get; set; }

        public virtual Status Status { get; set; }

        public virtual ProjectNh Project { get; set; }

        public virtual ISet<EmployeeNh> Employees { get; set; }
    }
}
