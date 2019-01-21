using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingTask.Web.MVC.Models
{
    public class ProjectCreationModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        [StringLength(200, MinimumLength = 0)]
        public string Description { get; set; }

        public string Tasks { get; set; }
    }
}
