using Optsol.Repository.Base.Pagination;

namespace Optsol.Repository.Base;

public interface IPaginatedReadRepository<T>
{
    ISearchResult<T> GetAll<TSearch>(ISearchRequest<TSearch> searchRequest) where TSearch : class;
}