using Optsol.Domain.Entities;

namespace Optsol.Repository;

public interface IReadRepository<TAggregateRoot> : Base.IReadRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
{
    TAggregateRoot GetById(Guid id);

    IEnumerable<TAggregateRoot> GetAllByIds(params Guid[] ids);
}