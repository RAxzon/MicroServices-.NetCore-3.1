using Ordering.Core.Entities.Interfaces;

namespace Ordering.Core.Entities.Base
{
    public abstract class EntityBase<TId> : IEntityBase<TId>
    {
        public virtual TId Id { get; protected set; }

        private int? _requestHashCode;

        public bool IsTransient()
        {
            return Id.Equals(default(TId));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EntityBase<TId>))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var item = (EntityBase<TId>) obj;

            if (item.IsTransient() || IsTransient())
            {
                return false;
            }

            return item == this;
        }


        public override int GetHashCode()
        {
            if (IsTransient()) return base.GetHashCode();
            if (_requestHashCode.HasValue) return base.GetHashCode();
            _requestHashCode = Id.GetHashCode() ^ 31;

            return _requestHashCode.Value;
        }

        public static bool operator ==(EntityBase<TId> left, EntityBase<TId> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null) ? true : false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(EntityBase<TId> left, EntityBase<TId> right)
        {
            return !(left == right);
        }
    }
}
