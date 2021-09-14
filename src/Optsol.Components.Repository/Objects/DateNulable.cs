using System;

namespace Optsol.Components.Repository.Domain.Objects
{
    public class DateNulable
    {
        public DateTime? Date { get; }

        public bool DateHasValue() => Date.HasValue;
    }
}
