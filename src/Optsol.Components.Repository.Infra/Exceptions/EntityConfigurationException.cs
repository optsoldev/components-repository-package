using System;

namespace Optsol.Components.Repository.Infra.EFCore.Exceptions
{
    [Serializable]
    internal class EntityConfigurationException : Exception
    {
        public EntityConfigurationException(string message) 
            : base(message)
        {
        }
    }
}