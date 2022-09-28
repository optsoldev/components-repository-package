using System;
using System.Linq;
using Optsol.Domain.Entities;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories.Pagination
{
    public interface IInclude<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> Include();
    }
}
