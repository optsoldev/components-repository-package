using Optsol.Components.Repository.Domain.ValueObjects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public abstract class Entity : IEntity
    {
        public Key Key { get; protected set; }

        protected Entity()
        {
            Key = Key.Create(Guid.NewGuid());
        }

        public bool Equals(IEntity other)
        {
            if (other is null) return false;

            return GetType() == other.GetType() && Key == other.Key;
        }

        public override bool Equals(object obj) => Equals(obj as IEntity);

        public override int GetHashCode() => $"{GetType()}{Key.GetHashCode()}".GetHashCode();
    }
}
