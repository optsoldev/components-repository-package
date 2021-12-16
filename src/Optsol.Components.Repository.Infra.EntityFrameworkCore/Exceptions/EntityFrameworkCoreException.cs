using System;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Exceptions
{
    public class EntityFrameworkCoreException : Exception
    {
        public EntityFrameworkCoreException(string message) : base(message)
        {
        }
    }
}
