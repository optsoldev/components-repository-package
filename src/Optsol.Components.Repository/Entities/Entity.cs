using Optsol.Components.Repository.Domain.Objects;
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

        public abstract bool Equals(IEntity other);
    }
}
