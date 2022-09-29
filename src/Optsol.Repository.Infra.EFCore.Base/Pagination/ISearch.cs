using System.Linq.Expressions;

namespace Optsol.Repository.Infra.EFCore.Base.Pagination;

public interface ISearch<T> 
{
    Expression<Func<T, bool>> Searcher();
}