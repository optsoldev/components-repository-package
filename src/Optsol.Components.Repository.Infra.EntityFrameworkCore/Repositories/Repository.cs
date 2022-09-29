using Microsoft.EntityFrameworkCore;
using Optsol.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories.Pagination;
using Optsol.Repository;
using Optsol.Repository.Base.Pagination;
using Optsol.Repository.Infra.EFCore.Base.Contexts;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories;

    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected Context Context { get; init; }

        protected DbSet<TAggregateRoot> Set { get; }

        protected Repository(Context context)
        {
            Context = context;
            Set = context.Set<TAggregateRoot>();
        }

        public virtual TAggregateRoot GetById(Guid id) => Set.Find(id);

        public virtual IEnumerable<TAggregateRoot> GetAllByIds(params Guid[] ids)
            => Set.Where(aggregate => ids.Any(key => key == aggregate.Id));
    
        public virtual IEnumerable<TAggregateRoot> GetAll() => Set.AsEnumerable();

        public virtual IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, bool>> filterExpression)
            => Set.Where(filterExpression.Compile());
        
        public ISearchResult<TAggregateRoot> GetAll<TSearch>(ISearchRequest<TSearch> searchRequest)
            where TSearch : class
        {
            var search = searchRequest.Search as ISearch<TAggregateRoot>;
            var include = searchRequest.Search as IInclude<TAggregateRoot>;
            var orderBy = searchRequest.Search as IOrderBy<TAggregateRoot>;

            var page = searchRequest.Page is not null && searchRequest.Page.Value > 0 ? searchRequest.Page.Value : 1;
            var pageSize = searchRequest.PageSize is not null && searchRequest.PageSize.Value > 0
                ? searchRequest.PageSize.Value
                : 10;
            
            IQueryable<TAggregateRoot> query = this.Set;

            query = ApplySearch(query, search?.Searcher());

            query = ApplyInclude(query, include?.Include());

            query = ApplyOrderBy(query, orderBy?.OrderBy());

            return new SearchResult<TAggregateRoot>()
                .SetPage(page)
                .SetPageSize(searchRequest.PageSize)
                .SetTotalCount(query.Count())
                .SetPaginatedItems(ApplyPagination(query, page, pageSize));
        }

        public virtual void Insert(TAggregateRoot aggregate) => Set.Add(aggregate);

        public virtual void InsertRange(IList<TAggregateRoot> aggregates) => Set.AddRange(aggregates);

        public virtual void Update(TAggregateRoot aggregate)
        {
            var aggregateInLocal = Set.Local.FirstOrDefault(localEntity => localEntity.Id == aggregate.Id);
            if (aggregateInLocal is not null)
            {
                Context.Entry(aggregateInLocal).State = EntityState.Detached;
            }

            Set.Update(aggregate);
        }

        public virtual void Delete(TAggregateRoot aggregate)
        {
            if (aggregate is null) return;

            if (aggregate is IEntityDeletable deletable)
            {
                deletable.Delete();

                Update(aggregate);

                return;
            }

            Set.Attach(aggregate).State = EntityState.Detached;
        }

        public virtual void DeleteRange(IList<TAggregateRoot> aggregates)
        {
            foreach (var aggregate in aggregates)
            {
                Delete(aggregate);
            }
        }

        public virtual int SaveChanges() => Context.SaveChanges();

        private static IEnumerable<TAggregateRoot> ApplyPagination(IQueryable<TAggregateRoot> query, int page, int size)
            => query.Skip(--page * size).Take(size).AsEnumerable();
      
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