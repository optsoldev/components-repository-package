using Optsol.Components.Repository.Domain.Objects;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntityCreatable : IEntity
    {
        DateValue CreateDate { get; }
    }
}
