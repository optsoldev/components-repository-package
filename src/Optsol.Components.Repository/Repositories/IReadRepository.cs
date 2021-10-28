using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IReadRepository<TEntity>
        where TEntity : IAggregateRoot
    {
        TEntity GetByKey(Key key);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAllByKeys(params Key[] keys);

        IEnumerable<TEntity> GetWithExpression(Expression<Func<TEntity, bool>> filterExpression);
    }
}