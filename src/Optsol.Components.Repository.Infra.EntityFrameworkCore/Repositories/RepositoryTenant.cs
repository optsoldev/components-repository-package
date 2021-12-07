using Microsoft.EntityFrameworkCore;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.Repositories;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Contexts;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Repositories
{
    public class RepositoryTenant<TEntity> : Repository<TEntity>, IRepository<TEntity>
        where TEntity : class, IAggregateRoot, ITentantProvider, IDisposable
    {
        public ITentantProvider TentantProvider { get; protected set; }

        public RepositoryTenant(Context context, ITentantProvider tentantProvider)
            : base(context)
        {
            Context = context;
            TentantProvider = tentantProvider;
        }


        public override void Insert(TEntity entity)
        {
            ((IEntityTenantable)entity).SetTenantKey(TentantProvider.GetTenantId());

            Set.Add(entity);
        }
    }
}
