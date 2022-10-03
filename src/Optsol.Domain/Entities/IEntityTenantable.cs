using Optsol.Domain.ValueObjects;
using System;

namespace Optsol.Domain.Entities
{
    public interface IEntityTenantable
    {
        Guid TentantKey { get; }

        void SetTenantKey(Guid tentantKey);
    }
}
