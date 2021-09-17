using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Repository.Domain.Entities;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.EFCore.Extensions
{
    public static class EntityConfigurationExtensions
    {
        public static EntityTypeBuilder<TEntity> BuildKey<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : AggregateRoot
        {
            builder.OwnsOne(entity => entity.Id, key =>
            {
                key.HasKey(key => key.Id);
            });

            return builder;
        }

        public static EntityTypeBuilder<TEntity> BuildCreatable<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : AggregateRoot
        {
            builder.OwnsOne(entity => entity.CreateDate, createDate =>
            {
                createDate.Property(date => date.Date)
                    .HasColumnName(nameof(IEntityCreatable.CreateDate))
                    .HasColumnName("datetime")
                    .IsRequired();
            });

            return builder;
        }

        public static EntityTypeBuilder<TEntity> BuildDeletable<TEntity>(this EntityTypeBuilder<TEntity> builder)
           where TEntity : AggregateRoot
        {
            builder.OwnsOne(entity => ((IEntityDeletable)entity).DeletedDate, createDate =>
            {
                createDate.Property(date => date.Date)
                    .HasColumnName(nameof(IEntityDeletable.DeletedDate))
                    .HasColumnName("datetime");
            });

            return builder;
        }

        public static EntityTypeBuilder<TEntity> BuildTenantable<TEntity>(this EntityTypeBuilder<TEntity> builder)
           where TEntity : AggregateRoot
        {
            builder.OwnsOne(entity => ((IEntityTenantable)entity), createDate =>
            {
                createDate.Property(date => date.TentantKey)
                    .HasColumnName(nameof(IEntityTenantable.TentantKey))
                    .IsRequired();
            });

            return builder;
        }
    }
}
