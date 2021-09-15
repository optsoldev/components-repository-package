using Optsol.Components.Repository.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Domain.ValueObjects
{
    public sealed class Key : ValueObject
    {
        public object Id { get; private set; }

        public Guid AsGuid()
        {
            try
            {
                return Guid.ParseExact(Id?.ToString(), "B");
            }
            catch (ArgumentNullException)
            {
                throw new KeyException($"{nameof(Id)} is null");
            }
            catch (FormatException)
            {
                throw new KeyException($"{nameof(Id)} bad format: {Id}");
            }
        }

        public static Key Create(object id)
        {
            Key newCreateKey = new();
            newCreateKey.Id = id;

            return newCreateKey;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
