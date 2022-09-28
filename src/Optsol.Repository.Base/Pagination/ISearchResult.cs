namespace Optsol.Repository.Base.Pagination;

public interface ISearchResult<T>
{
    public int Page { get;  }

    public int? PageSize { get;  }

    public long TotalCount { get; }

    public long PageCount => Items.Count();

    public IEnumerable<T> Items { get; }

    public ISearchResult<T> SetPage(int page);

    public ISearchResult<T> SetPageSize(int? pageSize);

    public ISearchResult<T> SetTotalCount(int totalCount);

    public ISearchResult<T> SetPaginatedItems(IEnumerable<T> items);
}