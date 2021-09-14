using Optsol.Components.Repository.Domain.Entities;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IRepository<TEntity> :
        IReadRepository<TEntity>,
        IWriteRepository<TEntity>
        where TEntity : IAggregateRoot
    { }
}
