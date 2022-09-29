using System;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Providers
{
    public interface ITentantProvider
    {
        Guid GetTenantId();
    }
}
