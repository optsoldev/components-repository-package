using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.Mock.Repositories
{
    public class MockRepository<TEntity> : IRepository<TEntity>
        where TEntity : IAggregateRoot
    {
        public IEnumerable<TEntity> Entities { get; private set; }

        public MockRepository(IEnumerable<TEntity> entities)
        {
            Entities = entities;
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAllByIds(params Key[] ids)
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(Key key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetWithExpression(Expression<Func<TEntity, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void Inset(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
