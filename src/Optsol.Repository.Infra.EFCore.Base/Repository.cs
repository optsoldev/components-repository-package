using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Optsol.Repository.Base;
using Optsol.Repository.Base.Pagination;
using Optsol.Repository.Infra.EFCore.Base.Contexts;
using Optsol.Repository.Infra.EFCore.Base.Pagination;

namespace Optsol.Repository.Infra.EFCore.Base;

public abstract class Repository<T> : IRepository<T> where T : class
{
        protected Context Context { get; init; }

        protected DbSet<T> Set { get; }

        protected Repository(Context context)
        {
            Context = context;
            Set = context.Set<T>();
        }
        
        public virtual IEnumerable<T> GetAll() => Set.AsEnumerable();

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression)
            => Set.Where(filterExpression.Compile());
        
        public ISearchResult<T> GetAll<TSearch>(ISearchRequest<TSearch> searchRequest)
            where TSearch : class
        {
            var search = searchRequest.Search as ISearch<T>;
            var include = searchRequest.Search as IInclude<T>;
            var orderBy = searchRequest.Search as IOrderBy<T>;

            var page = searchRequest.Page is not null && searchRequest.Page.Value > 0 ? searchRequest.Page.Value : 1;
            var pageSize = searchRequest.PageSize is not null && searchRequest.PageSize.Value > 0
                ? searchRequest.PageSize.Value
                : 10;
            
            IQueryable<T> query = this.Set;

            query = ApplySearch(query, search?.Searcher());

            query = ApplyInclude(query, include?.Include());

            query = ApplyOrderBy(query, orderBy?.OrderBy());

            return new SearchResult<T>()
                .SetPage(page)
                .SetPageSize(searchRequest.PageSize)
                .SetTotalCount(query.Count())
                .SetPaginatedItems(ApplyPagination(query, page, pageSize));
        }

        public virtual void Insert(T aggregate) => Set.Add(aggregate);

        public virtual void InsertRange(IList<T> aggregates) => Set.AddRange(aggregates);
    
        public virtual int SaveChanges() => Context.SaveChanges();

        private static IEnumerable<T> ApplyPagination(IQueryable<T> query, int page, int size)
            => query.Skip(--page * size).Take(size).AsEnumerable();
      
        private static IQueryable<T> ApplySearch(IQueryable<T> query, Expression<Func<T, bool>>? search = null)
        {
            var searchIsNotNull = search != null;
            if (searchIsNotNull)
            {
                if (search != null) query = query.Where(search);
            }

            return query;
        }

        private static IQueryable<T> ApplyInclude(IQueryable<T> query, Func<IQueryable<T>, IQueryable<T>>? includes = null)
        {
            if (includes is not null)
            {
                query = includes(query);
            }

            return query;
        }

        private static IQueryable<T> ApplyOrderBy(IQueryable<T> query, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            return query;
        }
    }