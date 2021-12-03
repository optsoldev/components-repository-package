using System;

namespace Optsol.Components.Repository.Infra.Mock.Exceptions
{
    public class CreditCardException : Exception
    {
        public CreditCardException(string message) : base(message)
        {
        }
    }
}
