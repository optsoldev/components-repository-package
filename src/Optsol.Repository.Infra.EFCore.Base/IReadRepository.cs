namespace Optsol.Repository.Infra.EFCore.Base;

public interface IReadRepository<T> : Optsol.Repository.Base.IReadRepository<T>
{
    Task<IEnumerable<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> Includes);
}