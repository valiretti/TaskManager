using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingTask.Common.Models
{
    /// <summary>
    /// Represents a model for creating project
    /// </summary>
    public class CreateProject
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        [StringLength(200, MinimumLength = 0)]
        public string Description { get; set; }

        public IEnumerable<CreateTask> Tasks { get; set; }
    }
}
