using System;

namespace Optsol.Components.Repository.Domain.Exceptions
{
    public class DateNullableException : Exception
    {
        public DateNullableException(string message) : base(message)
        {
        }
    }
}
