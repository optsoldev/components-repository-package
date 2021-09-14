using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Objects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Domain.Repositories
{
    public interface IReadRepository<TEntity>
        where TEntity : IAggregateRoot
    {
        TEntity GetById(Key key);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAllByIds(params Key[] ids);

        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);

        IEnumerable<TEntity> GetWithExpression(Expression<Func<TEntity, bool>> filterExpression);
    }
}
