using Optsol.Components.Repository.Domain.Objects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public Key Id { get; }

        public DateValue CreateDate { get; }

        public AggregateRoot()
        {
            Id = Key.Create(Guid.NewGuid());
            CreateDate = DateValue.Create();
        }

        public abstract bool Equals(IEntity other);
    }
}
