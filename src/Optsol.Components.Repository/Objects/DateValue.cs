using System;

namespace Optsol.Components.Repository.Domain.Objects
{
    public class DateValue
    {
        public DateTime Date { get; private set; }

        public void SetDateValueWithDateOfNow()
        {
            Date = DateTime.Now;
        }

        public static DateValue Create()
        {
            DateValue newDateValue = new();
            newDateValue.SetDateValueWithDateOfNow();

            return newDateValue;
        }

        public override string ToString()
        {
            return Date.ToShortDateString();
        }
    }
}
