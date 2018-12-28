using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingTask.Common.Models
{
    public class CreateProject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public IEnumerable<CreateTask> Tasks { get; set; }
    }
}
