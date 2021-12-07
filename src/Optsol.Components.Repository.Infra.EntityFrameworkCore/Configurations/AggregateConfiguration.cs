using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Repository.Domain.Entities;
using Optsol.Components.Repository.Infra.EntityFrameworkCore.Extensions;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Configurations
{
    public class AggregateConfiguration<TAggregate> : IEntityTypeConfiguration<TAggregate>
        where TAggregate : AggregateRoot
    {
        public virtual void Configure(EntityTypeBuilder<TAggregate> builder)
        {
            builder.BuildKey();
            builder.BuildCreatable();         
            builder.BuildDeletable();
            builder.BuildTenantable();            
            builder.BuildQueryFilter();
        }
    }
}
