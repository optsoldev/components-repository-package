using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Domain.ValueObjects;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Exceptions;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Extensions
{
    public static class EntityConfigurationExtensions
    {
        public static EntityTypeBuilder<TAggregateRoot> BuildKey<TAggregateRoot>(this EntityTypeBuilder<TAggregateRoot> builder)
            where TAggregateRoot : AggregateRoot
        {
            builder.OwnsOne(entity => entity.Key, key =>
            {
                key.HasKey(key => key.Id);
            });

            return builder;
        }

        public static EntityTypeBuilder<TAggregateRoot> BuildCreatable<TAggregateRoot>(this EntityTypeBuilder<TAggregateRoot> builder)
            where TAggregateRoot : AggregateRoot
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

        public static EntityTypeBuilder<TAggregateRoot> BuildDeletable<TAggregateRoot>(this EntityTypeBuilder<TAggregateRoot> builder)
           where TAggregateRoot : AggregateRoot
        {
            if (typeof(TAggregateRoot).AggregateIsDeletable() is not true)
                return builder;

            builder.OwnsOne(entity => ((IEntityDeletable)entity).DeletedDate, createDate =>
            {
                createDate.Property(date => date.Date)
                    .HasColumnName(nameof(IEntityDeletable.DeletedDate))
                    .HasColumnName("datetime");
            });

            return builder;
        }

        public static EntityTypeBuilder<TAggregateRoot> BuildTenantable<TAggregateRoot>(this EntityTypeBuilder<TAggregateRoot> builder)
           where TAggregateRoot : AggregateRoot
        {
            if (typeof(TAggregateRoot).AggregateIsTenantable() is not true)
                return builder;

            builder.OwnsOne(entity => ((IEntityTenantable)entity), createDate =>
            {
                createDate.Property(date => date.TentantKey)
                    .HasColumnName(nameof(IEntityTenantable.TentantKey))
                    .IsRequired();
            });

            return builder;
        }

        public static EntityTypeBuilder<TAggregateRoot> BuildQueryFilter<TAggregateRoot>(this EntityTypeBuilder<TAggregateRoot> builder, ITentantProvider tentnatProvider = null)
            where TAggregateRoot : AggregateRoot
        {
            Expression<Func<TAggregateRoot, bool>> @default = aggregateRoot => true;

            var expression = Expression.Lambda(@default);
            var aggregateParameter = Expression.Parameter(typeof(TAggregateRoot), "aggregateRoot");

            expression = QueryFilterDeletable<TAggregateRoot>(expression, aggregateParameter);
            expression = QueryFilterTenantable<TAggregateRoot>(expression, aggregateParameter, tentnatProvider);

            return builder.HasQueryFilter(expression);
        }

        private static LambdaExpression QueryFilterTenantable<TAggregateRoot>(LambdaExpression expression, ParameterExpression aggregateParameter, ITentantProvider tentnatProvider) where TAggregateRoot : AggregateRoot
        {
            if (typeof(TAggregateRoot).AggregateIsTenantable() is not true)
            {
                return expression;
            }

            if (tentnatProvider is null)
            {
                throw new EntityConfigurationException($"{nameof(tentnatProvider)} is required");
            }

            var innerExpression = Expression.Lambda<Func<AggregateRoot, bool>>(
                Expression.Equal(Expression.Property(aggregateParameter, nameof(IEntityTenantable.TentantKey)),
                Expression.Constant(tentnatProvider.GetTenantKey().Id)),
                aggregateParameter);

            return Expression.Lambda<Func<TAggregateRoot, bool>>(Expression.AndAlso(expression.Body, innerExpression.Body), innerExpression.Parameters);
        }

        private static LambdaExpression QueryFilterDeletable<TAggregateRoot>(LambdaExpression expression, ParameterExpression aggregateParameter) where TAggregateRoot : AggregateRoot
        {
            if (typeof(TAggregateRoot).AggregateIsDeletable() is not true)
                return expression;

            var innerExpression = Expression.Lambda<Func<AggregateRoot, bool>>(
                Expression.Equal(Expression.Property(aggregateParameter, nameof(IEntityDeletable.IsDeleted)),
                Expression.Constant(false)),
                aggregateParameter);

            return Expression.Lambda<Func<TAggregateRoot, bool>>(Expression.AndAlso(expression.Body, innerExpression.Body), innerExpression.Parameters);
        }

        private static bool AggregateIsDeletable(this Type aggregateType) => aggregateType.AggregateIs(typeof(IEntityDeletable));

        private static bool AggregateIsTenantable(this Type aggregateType) => aggregateType.AggregateIs(typeof(IEntityTenantable));

        private static bool AggregateIs(this Type aggregateType, Type type) => aggregateType.GetInterfaces().Contains(type);
    }
}
