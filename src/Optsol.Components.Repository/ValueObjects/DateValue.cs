using System;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Domain.ValueObjects
{
    public class DateValue : ValueObject
    {
        public DateTime Date { get; private set; }

        public void SetDateValueWithDateOfNow()
        {
            Date = DateTime.Now;
        }

        public override string ToString()
        {
            return Date.ToShortDateString();
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
        }

        public static DateValue Create()
        {
            DateValue newDateValue = new();
            newDateValue.SetDateValueWithDateOfNow();

            return newDateValue;
        }
    }
}
