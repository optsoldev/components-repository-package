namespace Optsol.Repository.Base.Pagination;

public class SearchResult<T> : ISearchResult<T>
{
    public int Page { get; private set; }

    public int? PageSize { get; private set; }

    public long TotalCount { get; private set; }

    public long PageCount => Items?.Count() ?? 0;

    public IEnumerable<T>? Items { get; private set; }

    public ISearchResult<T> SetPage(int page)
    {
        Page = page;

        return this;
    }

    public ISearchResult<T> SetPageSize(int? pageSize)
    {
        PageSize = pageSize;

        return this;
    }

    public ISearchResult<T> SetTotalCount(int totalCount)
    {
        TotalCount = totalCount;

        return this;
    }

    public ISearchResult<T> SetPaginatedItems(IEnumerable<T>? items)
    {
        Items = items;

        return this;
    }
}