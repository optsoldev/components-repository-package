namespace Optsol.Components.Repository.Domain.Repositories.Pagination
{
    public class SearchRequest<TSearch>
        where TSearch : class
    {
        public SearchRequest(TSearch search, uint page, uint? size)
        {
            Page = page;
            Size = size;
            Search = search;
        }

        public uint Page { get; private set; }

        public uint? Size { get; private set; }

        public TSearch Search { get; private set; }
    }
}
