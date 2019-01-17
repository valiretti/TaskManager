using System.ComponentModel.DataAnnotations;

namespace TrainingTask.Web.MVC.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        public string Patronymic { get; set; }

        [Required]
        public Position Position { get; set; }
    }
}
