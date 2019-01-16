using System.ComponentModel.DataAnnotations;

namespace TrainingTask.Web.MVC.Models
{
    public enum Status
    {
        [Display(Name = "Not Started")]
        NotStarted = 0,
        [Display(Name = "In Progress")]
        InProgress = 1,
        Completed = 2,
        Postponed = 3
    }
}
