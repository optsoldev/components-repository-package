using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories.Pagination;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IReadRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        TAggregateRoot GetById(Guid id);

        IEnumerable<TAggregateRoot> GetAll();

        IEnumerable<TAggregateRoot> GetAllByIds(params Guid[] ids);
    }

    public interface IExpressionReadRepository<TAggregateRoot> : IReadRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, bool>> filterExpression);
    }

    public interface IPaginatedReadRepository<TAggregateRoot> : IExpressionReadRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        SearchResult<TAggregateRoot> GetAll<TSearch>(SearchRequest<TSearch> searchRequest)
            where TSearch : class;
    }
}