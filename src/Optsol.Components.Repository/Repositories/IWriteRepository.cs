using Optsol.Components.Repository.Domain.Entities;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IWriteRepository<TEntity>
        where TEntity: IAggregateRoot 
    {
        void Inset(TEntity entity);
        
        void Update(TEntity entity);
        
        void Delete(TEntity entity);

        int SaveChanges();
    }
}
