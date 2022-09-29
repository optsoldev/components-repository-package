namespace Optsol.Repository.Infra.EFCore.Base.Pagination;

public interface IOrderBy<T>
{
    Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy();
}
