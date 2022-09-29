using System;
using Optsol.Domain.ValueObjects;

namespace Optsol.Domain.Entities
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
