using Optsol.Domain.ValueObjects;

namespace Optsol.Domain.Entities
{
    public interface IEntityCreatable : IEntity
    {
        DateValue CreatedDate { get; }
    }
}
