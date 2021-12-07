using Optsol.Components.Repository.Domain.ValueObjects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        public DateValue CreatedDate { get; private set; }

        public AggregateRoot()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateValue.Create().SetDateValueWithDateOfNow();
        }

        public override bool Equals(object obj) => Equals(obj as IAggregateRoot);

        public override int GetHashCode() => $"{GetType()}{Id.GetHashCode()}".GetHashCode();
    }
}
