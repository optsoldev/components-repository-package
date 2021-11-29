using System;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Exceptions
{
    public class EntityConfigurationException : Exception
    {
        public EntityConfigurationException(string message) : base(message)
        {
        }
    }
}
