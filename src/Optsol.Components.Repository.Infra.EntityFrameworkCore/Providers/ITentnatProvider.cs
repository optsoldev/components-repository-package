using Optsol.Components.Repository.Domain.ValueObjects;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Providers
{
    public interface ITentantProvider
    {
        Key GetTenantKey();
    }
}
