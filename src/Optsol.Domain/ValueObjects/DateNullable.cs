using System;
using System.Collections.Generic;
using Optsol.Domain.Exceptions;

namespace Optsol.Domain.ValueObjects
{
    public sealed class DateNullable : ValueObject
    {
        public DateTime? Date { get; private set; }

        public DateNullable()
        {
            Date = null;
        }

        public DateNullable SetDateValueWithDateOfNow()
        {
            Date = DateTime.Now;

            return this;
        }

        public DateNullable SetDateValueWithDate(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                throw new DateValueException($"{Date} not is min value");
            }

            Date = date;

            return this;
        }

        public bool DateHasValue() => Date.HasValue;

        public override string ToString()
        {
            if (Date is null)
                return "null";

            return Date.Value.ToShortDateString();
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
        }

        public static DateNullable Create() => new();
    }
}
