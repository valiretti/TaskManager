using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainingTask.Common.Enums;

namespace TrainingTask.Web.MVC.Models
{
    public class TaskCreationModel
    {

        public int Id { get; set; }

        [Display(Name = "Project")]
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Work time(hours)")]
        public double WorkHours { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Finish Date")]
        public DateTime FinishDate { get; set; }

        [Required]
        public Status Status { get; set; }

        public IEnumerable<int> Employees { get; set; }
    }
}
