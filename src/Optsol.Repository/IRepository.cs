using Optsol.Domain.Entities;

namespace Optsol.Repository
{
    public interface IRepository<TAggregateRoot> :
        IReadRepository<TAggregateRoot>,
        IExpressionReadRepository<TAggregateRoot>,
        IPaginatedReadRepository<TAggregateRoot>,
        IWriteRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    { }
}
