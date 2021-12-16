using Optsol.Components.Repository.Domain.Entities;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IRepository<TAggregateRoot> :
        IReadRepository<TAggregateRoot>,
        IExpressionReadRepository<TAggregateRoot>,
        IPaginatedReadRepository<TAggregateRoot>,
        IWriteRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    { }
}
