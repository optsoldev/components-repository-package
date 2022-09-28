using Optsol.Domain.Entities;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Contexts;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Providers;
using System;
using Optsol.Repository;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories
{
    public class RepositoryTenant<TAggregateRoot> : Repository<TAggregateRoot>, IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, ITentantProvider, IDisposable
    {
        public ITentantProvider TentantProvider { get; protected set; }

        public RepositoryTenant(Context context, ITentantProvider tentantProvider)
            : base(context)
        {
            Context = context;
            TentantProvider = tentantProvider;
        }


        public override void Insert(TAggregateRoot entity)
        {
            ((IEntityTenantable)entity).SetTenantKey(TentantProvider.GetTenantId());

            Set.Add(entity);
        }
    }
}
