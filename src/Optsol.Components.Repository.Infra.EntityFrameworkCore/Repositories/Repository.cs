using Microsoft.EntityFrameworkCore;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.Repositories.Pagination;
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
            return Set.Where(aggregate => ids.Any(key => key == aggregate.Id));
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
        }

        public virtual void Insert(TAggregateRoot aggregate) => Set.Add(aggregate);

        public virtual void InsertRange(List<TAggregateRoot> aggregates) => Set.AddRange(aggregates);

        public virtual void Update(TAggregateRoot aggregate)
        {
            var aggregateInLocal = Set.Local?.FirstOrDefault(localEntity => localEntity.Id == aggregate.Id);
            if (aggregateInLocal is not null)
            {
                Context.Entry(aggregateInLocal).State = EntityState.Detached;
            }

            Set.Update(aggregate);
        }

        public virtual void Delete(TAggregateRoot aggregate)
        {
            if (aggregate is null) return;

            if (aggregate is IEntityDeletable)
            {
                (aggregate as IEntityDeletable).Delete();

                Update(aggregate);

                return;
            }

            Set.Attach(aggregate).State = EntityState.Detached;
        }

        public virtual void DeleteRange(List<TAggregateRoot> aggregates)
        {
            foreach (var aggregate in aggregates)
            {
                Delete(aggregate);
            }
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