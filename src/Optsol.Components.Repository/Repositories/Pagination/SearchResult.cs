using Optsol.Components.Repository.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Repository.Domain.Repositories.Pagination
{
    public class SearchResult<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        public uint Page { get; private set; }

        public uint? Size { get; private set; }

        public long TotalCount { get; private set; }

        public long PageCount => Items.Count();
        
        public IEnumerable<TAggregateRoot> Items { get; private set; }

        public SearchResult<TAggregateRoot> SetPage(uint page)
        {
            Page = page;

            return this;
        }

        public SearchResult<TAggregateRoot> SetSize(uint? size)
        {
            Size = size;

            return this;
        }

        public SearchResult<TAggregateRoot> SetTotalCount(int totalCount)
        {
            TotalCount = totalCount;

            return this;
        }

        public SearchResult<TAggregateRoot> SetPaginatedItems(IEnumerable<TAggregateRoot> items) 
        {
            Items = items
                ;
            return this;
        }
    }
}
