using System;

namespace TrainingTask.DAL.Entities
{
    public abstract class BaseEntity : IEquatable<BaseEntity>
    {
        public virtual int Id { get; set; }

        public virtual bool Equals(BaseEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id != 0 && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseEntity)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
