using Optsol.Components.Repository.Domain.ValueObjects;
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

        public bool Equals(IEntity other)
        {
            if (other is null) return false;

            return GetType() == other.GetType() && Id == other.Id;
        }

        public override bool Equals(object obj) => Equals(obj as AggregateRoot);

        public override int GetHashCode() => Id.GetHashCode();
    }
}
