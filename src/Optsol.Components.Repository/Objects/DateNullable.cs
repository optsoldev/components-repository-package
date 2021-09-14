using Optsol.Components.Repository.Domain.Exceptions;
using System;

namespace Optsol.Components.Repository.Domain.Objects
{
    public class DateNullable
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

        public static DateNullable Create => new();
    }
}
