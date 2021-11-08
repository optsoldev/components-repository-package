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

    public static class KeyExtensions
    {
        public static Guid AsGuid(this Key key)
        {
            try
            {
                return Guid.ParseExact(key.Id?.ToString(), "B");
            }
            catch (NullReferenceException)
            {
                throw new KeyException($"{nameof(key.Id)} is null");
            }
            catch (FormatException)
            {
                throw new KeyException($"{nameof(key.Id)} bad format: {key.Id}");
            }
        }
    }
}
