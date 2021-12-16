using Optsol.Components.Repository.Domain.Entities;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IWriteRepository<TAggregate>
        where TAggregate: IAggregateRoot 
    {
        void Insert(TAggregate aggregate);
        
        void Update(TAggregate aggregate);
        
        void Delete(TAggregate aggregate);

        int SaveChanges();
    }
}
