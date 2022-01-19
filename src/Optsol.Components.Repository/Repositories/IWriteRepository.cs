using Optsol.Components.Repository.Domain.Entities;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IWriteRepository<TAggregate>
        where TAggregate : IAggregateRoot
    {
        void Insert(TAggregate aggregate);

        void InsertRange(List<TAggregate> aggregates);

        void Update(TAggregate aggregate);

        void Delete(TAggregate aggregate);

        void DeleteRange(List<TAggregate> aggregates);

        int SaveChanges();
    }
}