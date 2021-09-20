using System;

namespace Optsol.Components.Repository.Infra.EFCore.Exceptions
{
    public class EntityConfigurationException : Exception
    {
        public EntityConfigurationException(string message) : base(message)
        {
        }
    }
}
