using System;

namespace Optsol.Components.Repository.Domain.Exceptions
{
    [Serializable]
    public class DateValueException : Exception
    {
        public DateValueException(string message) : base(message)
        {
        }
    }
}
