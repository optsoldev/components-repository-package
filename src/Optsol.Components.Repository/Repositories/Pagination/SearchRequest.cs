namespace Optsol.Components.Repository.Domain.Repositories.Pagination
{
    public class SearchRequest<TSearch>
        where TSearch : class
    {
        public SearchRequest(TSearch search, int page, int? size)
        {
            Page = page;
            pageSize = size;
            Search = search;
        }

        public int Page { get; private set; }

        public int? pageSize { get; private set; }

        public TSearch Search { get; private set; }
    }
}
