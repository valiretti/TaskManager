using System.ComponentModel.DataAnnotations;

namespace TrainingTask.Common.Models
{
    /// <summary>
    /// Represents employee
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Patronymic { get; set; }

        [Required]
        public string Position { get; set; }

    }
}
