using Optsol.Components.Repository.Domain.Entities;
using System;
using System.Linq;

namespace Optsol.Components.Repository.Domain.Repositories.Pagination
{
    public interface IInclude<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> Include();
    }
}
