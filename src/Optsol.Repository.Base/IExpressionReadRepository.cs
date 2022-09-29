using System.Linq.Expressions;

namespace Optsol.Repository.Base;

public interface IExpressionReadRepository<T>
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression);
}