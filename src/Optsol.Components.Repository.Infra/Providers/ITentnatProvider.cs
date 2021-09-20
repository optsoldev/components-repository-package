using Optsol.Components.Repository.Domain.ValueObjects;

namespace Optsol.Components.Repository.Infra.EFCore.Providers
{
    public interface ITentantProvider
    {
        Key GetTenantKey();
    }
}
