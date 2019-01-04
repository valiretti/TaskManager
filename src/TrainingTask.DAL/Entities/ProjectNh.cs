using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingTask.DAL.Entities
{
    public class ProjectNh
    {
        public ProjectNh()
        {
            Tasks = new List<TaskNh>();
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Abbreviation { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<TaskNh> Tasks { get; set; }
    }
}
