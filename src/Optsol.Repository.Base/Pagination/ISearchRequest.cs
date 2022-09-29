namespace Optsol.Repository.Base.Pagination;

public interface ISearchRequest<out TSearch>
{
    public int? Page { get; }

    public int? PageSize { get; }

    public TSearch Search { get; }
}