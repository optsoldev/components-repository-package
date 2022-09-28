using System;

namespace Optsol.Domain.Exceptions
{
    [Serializable]
    public class DateValueException : Exception
    {
        public DateValueException(string message) : base(message)
        {
        }
    }
}
