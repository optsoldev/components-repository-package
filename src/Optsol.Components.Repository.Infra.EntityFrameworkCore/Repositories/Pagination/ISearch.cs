using Optsol.Components.Repository.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.Repositories.Pagination
{
    public interface ISearch<TAggregateRoot> 
        where TAggregateRoot : IAggregateRoot
    {
        Expression<Func<TAggregateRoot, bool>> Searcher();
    }
}
