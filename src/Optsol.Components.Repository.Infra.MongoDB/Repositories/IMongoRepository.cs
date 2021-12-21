using MongoDB.Driver;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Infra.MongoDB.Contexts;

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
