namespace Optsol.Repository.Infra.EFCore.Base.Pagination;

public interface IInclude<T>
{
    Func<IQueryable<T>, IQueryable<T>> Include();
}