using Optsol.Domain.Entities;

namespace Optsol.Repository;

public interface IExpressionReadRepository<TAggregateRoot> : Base.IExpressionReadRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
}