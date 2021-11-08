using Optsol.Components.Repository.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Domain.ValueObjects
{
    public sealed class DateNullable : ValueObject
    {
        public DateTime? Date { get; private set; }

        public DateNullable SetDateValueWithDateOfNow()
        {
            Date = DateTime.Now;

            return this;
        }

        public bool DateHasValue() => Date.HasValue;

        public override string ToString()
        {
            return Date.HasValue ? Date?.ToShortDateString() : "null";
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
        }

        public static DateNullable Create() => new();
    }
}
