using System.Collections.Generic;

namespace TrainingTask.DAL.Entities
{
    public class ProjectNh : BaseEntity
    {
        public ProjectNh()
        {
            Tasks = new HashSet<TaskNh>();
        }

        public virtual string Name { get; set; }

        public virtual string Abbreviation { get; set; }

        public virtual string Description { get; set; }

        public virtual ISet<TaskNh> Tasks { get; set; }
    }
}
