namespace Optsol.Repository.Base;

public interface IWriteRepository<T>
{
    void Insert(T aggregate);

    void InsertRange(IList<T> aggregates);

    int SaveChanges();
}