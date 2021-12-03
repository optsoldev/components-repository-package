using Optsol.Components.Repository.Domain.ValueObjects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public abstract class Entity : IEntity
    {
        public Key Id { get; protected set; }

        protected Entity()
        {
            Id = Key.Create(Guid.NewGuid());
        }

        public bool Equals(IEntity other)
        {
            if (other is null) return false;

            return GetType() == other.GetType() && Id == other.Id;
        }

        public override bool Equals(object obj) => Equals(obj as IEntity);

        public override int GetHashCode() => $"{GetType()}{Id.GetHashCode()}".GetHashCode();
    }
}
