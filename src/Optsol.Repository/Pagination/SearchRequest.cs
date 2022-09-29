using Optsol.Repository.Base.Pagination;

namespace Optsol.Repository.Pagination;
public sealed class SearchRequest<TSearch> : ISearchRequest<TSearch>
{
    public SearchRequest(TSearch search, int? page, int? size)
    {
        Page = page;
        PageSize = size;
        Search = search;
    }

    public int? Page { get; }

    public int? PageSize { get; }

    public TSearch Search { get; }
}