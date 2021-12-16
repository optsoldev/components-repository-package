using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;

namespace Optsol.Components.Repository.Infra.MongoDB.Repositories
{
    public interface IMongoRepository<TAggregate> : IReadRepository<TAggregate>, IWriteRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
        public Context Context { get; }

        IMongoCollection<TAggregate> Set { get; }
    }
}
