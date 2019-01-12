using System;
using System.Collections.Generic;
using System.Text;
using TrainingTask.Common.Enums;

namespace TrainingTask.Common.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        public string ProjectAbbreviation { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string FullName { get; set; }

        public Status Status { get; set; }

        public int ProjectId { get; set; }

        public int? EmployeeId { get; set; }

        public TimeSpan WorkHours { get; set; }
    }
}
