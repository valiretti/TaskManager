using System;
using TrainingTask.Common.Enums;

namespace TrainingTask.Common.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TimeSpan WorkHours { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public Status Status { get; set; }

        public int ProjectId { get; set; }

    }
}
