using System;
using System.Linq;
using Optsol.Domain.Entities;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories.Pagination
{
    public interface IOrderBy<TAggregateRoot>
         where TAggregateRoot : IAggregateRoot
    {
        Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>> OrderBy();
    }
}
