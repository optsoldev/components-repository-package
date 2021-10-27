using System;

namespace Optsol.Components.Repository.Domain.Exceptions
{
    public class NotificationException : Exception
    {
        public NotificationException(string message) : base(message)
        {
        }
    }
}
