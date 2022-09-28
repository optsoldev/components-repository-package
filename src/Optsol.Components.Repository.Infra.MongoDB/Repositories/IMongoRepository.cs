using MongoDB.Driver;
using Optsol.Domain.Entities;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;
using Optsol.Repository;

namespace Optsol.Components.Repository.Infra.MongoDB.Repositories
{
    public interface IMongoRepository<TAggregateRoot> : 
        IReadRepository<TAggregateRoot>, 
        IPaginatedReadRepository<TAggregateRoot>,
        IWriteRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public Context Context { get; }

        IMongoCollection<TAggregateRoot> Set { get; }
    }
}
