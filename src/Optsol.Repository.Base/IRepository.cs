namespace Optsol.Repository.Base;

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> { }