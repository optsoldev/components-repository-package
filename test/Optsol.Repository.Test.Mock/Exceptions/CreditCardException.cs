using System;

namespace Optsol.Repository.Test.Mock.Exceptions
{
    public class CreditCardException : Exception
    {
        public CreditCardException(string message) : base(message)
        {
        }
    }
}
