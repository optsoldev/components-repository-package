using Optsol.Components.Repository.Domain.ValueObjects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntityTenantable
    {
        Guid TentantKey { get; }

        void SetTenantKey(Guid tentantKey);
    }
}
