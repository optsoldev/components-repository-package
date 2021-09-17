using Optsol.Components.Repository.Domain.ValueObjects;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntityTenantable
    {
        Key TentantKey { get; }

        void SetTenantKey(Key tentantKey);
    }
}
