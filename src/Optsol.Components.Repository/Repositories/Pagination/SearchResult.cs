using Optsol.Components.Repository.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Repository.Domain.Repositories.Pagination
{
    public class SearchResult<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        public int Page { get; private set; }

        public int? PageSize { get; private set; }

        public long TotalCount { get; private set; }

        public long PageCount => Items.Count();

        public IEnumerable<TAggregateRoot> Items { get; private set; }

        public SearchResult<TAggregateRoot> SetPage(int page)
        {
            Page = page;

            return this;
        }

        public SearchResult<TAggregateRoot> SetPageSize(int? pageSize)
        {
            PageSize = pageSize;

            return this;
        }

        public SearchResult<TAggregateRoot> SetTotalCount(int totalCount)
        {
            TotalCount = totalCount;

            return this;
        }

        public SearchResult<TAggregateRoot> SetPaginatedItems(IEnumerable<TAggregateRoot> items)
        {
            Items = items;

            return this;
        }
    }
}
