using Optsol.Domain.Entities;
using Optsol.Repository.Base.Pagination;

namespace Optsol.Repository.Pagination;
public class SearchResult<TAggregateRoot> : ISearchResult<TAggregateRoot> where TAggregateRoot : IAggregateRoot
{
    public int Page { get; private set; }

    public int? PageSize { get; private set; }

    public long TotalCount { get; private set; }

    public long PageCount => Items.Count();

    public IEnumerable<TAggregateRoot>? Items { get; private set; }

    public ISearchResult<TAggregateRoot> SetPage(int page)
    {
        Page = page;

        return this;
    }

    public ISearchResult<TAggregateRoot> SetPageSize(int? pageSize)
    {
        PageSize = pageSize;

        return this;
    }

    public ISearchResult<TAggregateRoot> SetTotalCount(int totalCount)
    {
        TotalCount = totalCount;

        return this;
    }

    public ISearchResult<TAggregateRoot> SetPaginatedItems(IEnumerable<TAggregateRoot>? items)
    {
        Items = items;

        return this;
    }
}