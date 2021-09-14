using System;

namespace Optsol.Components.Repository.Domain.Objects
{
    public class DateValue
    {
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Date.ToShortDateString();
        }
    }
}
