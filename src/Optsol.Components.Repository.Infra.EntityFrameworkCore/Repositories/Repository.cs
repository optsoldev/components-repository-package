using Microsoft.EntityFrameworkCore;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IAggregateRoot, IDisposable
    {
        public Context Context { get; protected set; }

        public DbSet<TEntity> Set { get; protected set; }

        public Repository(Context context)
        {
            Context = context;
            Set = context.Set<TEntity>(); 
        }

        public virtual TEntity GetById(Guid id) => Set.Find(id);

        public virtual IEnumerable<TEntity> GetAllByIds(params Guid[] ids)
        {
            return Set.Where(entity => ids.Any(key => key == entity.Id));
        }

        public virtual IEnumerable<TEntity> GetAll() => Set.AsEnumerable();

        public virtual IEnumerable<TEntity> GetWithExpression(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Set.Where(filterExpression.Compile());
        }

        public virtual void Insert(TEntity entity) => Set.Add(entity);

        public virtual void Update(TEntity entity)
        {
            var entityInLocal = Set.Local?.Any(localEntity => localEntity.Id == entity.Id) ?? false;

            if (entityInLocal)
            {
                Context.Entry(entity).State = EntityState.Detached;
            }

            Set.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity is null) return;

            if (entity is IEntityDeletable)
            {
                (entity as IEntityDeletable).Delete();

                Update(entity);

                return;
            }

            Set.Attach(entity).State = EntityState.Detached;
        }

        public virtual int SaveChanges() => Context.SaveChanges();
    }
}
