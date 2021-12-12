using Optsol.Components.Repository.Domain.Entities;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IRepository<TAggregate> :
        IReadRepository<TAggregate>,
        IExpressionReadRepository<TAggregate>,
        IWriteRepository<TAggregate>
        where TAggregate : IAggregateRoot
    { }
}
