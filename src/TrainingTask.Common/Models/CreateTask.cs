using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrainingTask.Common.Enums;

namespace TrainingTask.Common.Models
{
    public class CreateTask
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double WorkHours { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime FinishDate { get; set; }

        [Required]
        public Status Status { get; set; }

        public IEnumerable<int> Employees { get; set; }
    }
}
