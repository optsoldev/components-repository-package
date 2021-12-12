using Optsol.Components.Repository.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IReadRepository<TAggregate>
        where TAggregate : IAggregateRoot
    {
        TAggregate GetById(Guid id);

        IEnumerable<TAggregate> GetAll();

        IEnumerable<TAggregate> GetAllByIds(params Guid[] ids);
    }

    public interface IExpressionReadRepository<TAggregate> : IReadRepository<TAggregate>
        where TAggregate : IAggregateRoot
    { 
        IEnumerable<TAggregate> GetWithExpression(Expression<Func<TAggregate, bool>> filterExpression);
    }
}