using System;
using System.Collections.Generic;
using TrainingTask.Common.Enums;

namespace TrainingTask.Common.Models
{
    public class TaskViewModel 
    {
        public int Id { get; set; }

        public string ProjectAbbreviation { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public IEnumerable<string> FullNames { get; set; }

        public Status Status { get; set; }
    }
}
