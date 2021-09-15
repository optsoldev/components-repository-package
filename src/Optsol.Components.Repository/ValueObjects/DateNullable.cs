using Optsol.Components.Repository.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Domain.ValueObjects
{
    public sealed class DateNullable : ValueObject
    {
        public DateTime? Date { get; private set; }

        public void SetDateValueWithDateOfNow()
        {
            Date = DateTime.Now;
        }

        public bool DateHasValue() => Date.HasValue;

        public override string ToString()
        {
            return Date.HasValue ? Date?.ToShortDateString() : "00-00-0000";
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
        }

        public static DateNullable Create => new();
    }
}
