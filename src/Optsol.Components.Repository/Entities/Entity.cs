using Optsol.Components.Repository.Domain.ValueObjects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public abstract class Entity : IEntity
    {
        public Key Id { get; private set; }

        protected Entity()
        {
            Id = Key.Create(Guid.NewGuid());
        }

        public bool Equals(IEntity other)
        {
            if (other is null) return false;

            return GetType() == other.GetType() && Id == other.Id;
        }

        public override bool Equals(object obj) => Equals(obj as Entity);

        public override int GetHashCode() => Id.GetHashCode();
    }
}
