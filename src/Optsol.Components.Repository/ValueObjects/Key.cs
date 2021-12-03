using Optsol.Components.Repository.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Domain.ValueObjects
{
    public sealed class Key : ValueObject
    {
        public object Id { get; private set; }

        public static Key Create(object id) => new() { Id = id };

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
