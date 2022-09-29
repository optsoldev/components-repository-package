using Optsol.Domain.Entities;

namespace Optsol.Repository;

public interface IPaginatedReadRepository<TAggregateRoot> : Base.IPaginatedReadRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
}