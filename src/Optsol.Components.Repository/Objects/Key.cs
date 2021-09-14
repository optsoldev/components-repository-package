using Optsol.Components.Repository.Domain.Exceptions;
using System;

namespace Optsol.Components.Repository.Domain.Objects
{
    public class Key
    {
        public object Id { get; private set; }

        public Key(object id)
        {
            Id = id;
        }

        public Guid AsGuid()
        {
            try
            {
                return Guid.ParseExact(Id?.ToString(), "B");
            }
            catch (ArgumentNullException)
            {
                throw new KeyException($"{nameof(Id)} is null");
            }
            catch (FormatException)
            {
                throw new KeyException($"{nameof(Id)} Bab Format: {Id}");
            }
        }

        public static bool operator ==(Key left, Key right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Key left, Key right) => !(left == right);

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Id.Equals(((Key)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
