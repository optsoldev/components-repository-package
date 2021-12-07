using Optsol.Components.Repository.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IReadRepository<TEntity>
        where TEntity : IAggregateRoot
    {
        TEntity GetById(Guid id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAllByIds(params Guid[] ids);

        IEnumerable<TEntity> GetWithExpression(Expression<Func<TEntity, bool>> filterExpression);
    }
}