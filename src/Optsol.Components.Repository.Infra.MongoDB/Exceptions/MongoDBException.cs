using System;

namespace Optsol.Components.Repository.Infra.MongoDB.Exceptions
{
    public class MongoDBException : Exception
    {
        public MongoDBException(string message) : base(message)
        {
        }
    }
}
