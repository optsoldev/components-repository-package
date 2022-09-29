namespace Optsol.Repository.Base;

public interface IReadRepository<T> : IExpressionReadRepository<T>, IPaginatedReadRepository<T>
{
    IEnumerable<T> GetAll();
}