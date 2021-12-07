using Optsol.Components.Repository.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Domain.ValueObjects
{
    public class Key<KeyType> : ValueObject
    {
        public KeyType Id { get; protected set; }

        public static Key<KeyType> Create(KeyType id) => new() { Id = id };

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }

    public sealed class KeyGuid : Key<Guid>
    {
        public new static KeyGuid Create(Guid id) => new() { Id = id };

    }
}
