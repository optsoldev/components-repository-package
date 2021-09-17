using System;

namespace Optsol.Components.Repository.Domain.Exceptions
{
    [Serializable]
    public class KeyException : Exception
    {
        public KeyException(string message) : base(message)
        {
        }
    }
}
