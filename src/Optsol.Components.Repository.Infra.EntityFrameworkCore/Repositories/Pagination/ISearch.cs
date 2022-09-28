using System;
using System.Linq.Expressions;
using Optsol.Domain.Entities;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories.Pagination
{
    public interface ISearch<TAggregateRoot> 
        where TAggregateRoot : IAggregateRoot
    {
        Expression<Func<TAggregateRoot, bool>> Searcher();
    }
}
