using Microsoft.EntityFrameworkCore;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.Repositories.Pagination;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Contexts;
using Optsol.Components.Repository.Infra.Repositories.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories
{
    public class Repository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public Context Context { get; protected set; }

        public DbSet<TAggregateRoot> Set { get; protected set; }

        public Repository(Context context)
        {
            Context = context;
            Set = context.Set<TAggregateRoot>();
        }

        public virtual TAggregateRoot GetById(Guid id) => Set.Find(id);

        public virtual IEnumerable<TAggregateRoot> GetAllByIds(params Guid[] ids)
        {
            return Set.Where(entity => ids.Any(key => key == entity.Id));
        }

        public virtual IEnumerable<TAggregateRoot> GetAll() => Set.AsEnumerable();

        public virtual IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, bool>> filterExpression)
        {
            return Set.Where(filterExpression.Compile());
        }

        public SearchResult<TAggregateRoot> GetAll<TSearch>(SearchRequest<TSearch> searchRequest)
            where TSearch : class
        {
            var search = searchRequest.Search as ISearch<TAggregateRoot>;
            var include = searchRequest.Search as IInclude<TAggregateRoot>;
            var orderBy = searchRequest.Search as IOrderBy<TAggregateRoot>;

            IQueryable<TAggregateRoot> query = this.Set;

            query = ApplySearch(query, search?.Searcher());

            query = ApplyInclude(query, include?.Include());

            query = ApplyOrderBy(query, orderBy?.OrderBy());

            return new SearchResult<TAggregateRoot>()
                .SetPage(searchRequest.Page)
                .SetPageSize(searchRequest.pageSize)
                .SetTotalCount(query.Count())
                .SetPaginatedItems(ApplyPagination(query, searchRequest.Page, searchRequest.pageSize));
            ;

        }

        public virtual void Insert(TAggregateRoot entity) => Set.Add(entity);

        public virtual void Update(TAggregateRoot entity)
        {
            var entityInLocal = Set.Local?.Any(localEntity => localEntity.Id == entity.Id) ?? false;

            if (entityInLocal)
            {
                Context.Entry(entity).State = EntityState.Detached;
            }

            Set.Update(entity);
        }

        public virtual void Delete(TAggregateRoot entity)
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

        private static IEnumerable<TAggregateRoot> ApplyPagination(IQueryable<TAggregateRoot> query, int page, int? size)
        {
            var skip = --page * (size ?? 0);

            query = query.Skip(skip);

            if (size.HasValue)
            {
                query = query.Take(size.Value);
            }

            return query.AsEnumerable();
        }

        private static IQueryable<TAggregateRoot> ApplySearch(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> search = null)
        {
            var searchIsNotNull = search != null;
            if (searchIsNotNull)
            {
                query = query.Where(search);
            }

            return query;
        }

        private static IQueryable<TAggregateRoot> ApplyInclude(IQueryable<TAggregateRoot> query, Func<IQueryable<TAggregateRoot>, IQueryable<TAggregateRoot>> includes = null)
        {
            var includesIsNotNull = includes != null;
            if (includesIsNotNull)
            {
                query = includes(query);
            }

            return query;
        }

        private static IQueryable<TAggregateRoot> ApplyOrderBy(IQueryable<TAggregateRoot> query, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>> orderBy = null)
        {
            var orderByIsNotNull = orderBy != null;
            if (orderByIsNotNull)
            {
                query = orderBy(query);
            }

            return query;
        }
    }
}
