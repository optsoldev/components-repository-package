using Optsol.Domain.Entities;

namespace Optsol.Repository;

public interface IWriteRepository<TAggregate> : Base.IWriteRepository<TAggregate> where TAggregate : IAggregateRoot
{
    void Update(TAggregate aggregate);

    void Delete(TAggregate aggregate);

    void DeleteRange(IList<TAggregate> aggregates);
}
