using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Infra.EFCore.Exceptions;
using Optsol.Components.Repository.Infra.EFCore.Extensions;
using System;

namespace Optsol.Components.Repository.Infra.EFCore.Configurations
{
    public class ConfigurationOptions
    {
        internal bool configureDeletable = false;

        internal bool configureTenantable = false;

        public ConfigurationOptions EnabledConfigureDeletable()
        {
            configureDeletable = true;

            return this;
        }

        public ConfigurationOptions EnabledConfigureTenantable()
        {
            configureTenantable = true;

            return this;
        }
    }

    public abstract class Configuration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AggregateRoot
    {
        public ConfigurationOptions Options { get; set; } = new ConfigurationOptions();

        public abstract void Configure(Action<EntityTypeBuilder<TEntity>> options);

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.BuildKey();
            builder.BuildCreatable();

            if (Options.configureDeletable)
            {
                builder.BuildDeletable();
            }

            if (Options.configureTenantable)
            {
                builder.BuildTenantable();
            }
        }
    }
}
