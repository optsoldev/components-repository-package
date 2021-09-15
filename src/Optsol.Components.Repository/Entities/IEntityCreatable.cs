using Optsol.Components.Repository.Domain.ValueObjects;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntityCreatable : IEntity
    {
        DateValue CreateDate { get; }
    }
}
