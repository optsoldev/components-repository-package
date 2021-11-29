using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Repository.Domain.ValueObjects
{
    public abstract class ValueObject : IValueObject
    {
        public bool Equals(IValueObject other)
        {
            if (other is null) return false;

            return GetType() == other.GetType()
                && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public abstract IEnumerable<object> GetEqualityComponents();

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (left is null && right is null) return true;

            if (left is null ^ right is null) return false;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);

        public override bool Equals(object obj) => Equals(obj as ValueObject);

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(o => o is not null ? o.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
