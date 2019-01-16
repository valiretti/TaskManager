using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrainingTask.Common.Enums;

namespace TrainingTask.Web.MVC.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectAbbreviation { get; set; }

        public string Name { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Finish Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FinishDate { get; set; }

        [Display(Name = "Employees")]
        public IEnumerable<string> FullNames { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        public IEnumerable<int> Employees { get; set; }

        public double WorkHours { get; set; }
    }
}
