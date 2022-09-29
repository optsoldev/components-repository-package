using Microsoft.EntityFrameworkCore;
using Optsol.Repository.Base;
using Optsol.Repository.Infra.EFCore.Base.Contexts;

namespace Optsol.Repository.Infra.EFCore.Base;

public abstract class WriteRepository<T> : IWriteRepository<T> where T : class
{
    protected Context Context { get; init; }

    protected DbSet<T> Set { get; }

    protected WriteRepository(Context context)
    {
        Context = context;
        Set = context.Set<T>();
    }
    
    public virtual void Insert(T aggregate) => Set.Add(aggregate);

    public virtual void InsertRange(IList<T> aggregates) => Set.AddRange(aggregates);
    
    public virtual int SaveChanges() => Context.SaveChanges();
}