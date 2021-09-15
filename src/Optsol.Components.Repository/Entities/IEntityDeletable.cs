using Optsol.Components.Repository.Domain.ValueObjects;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntityDeletable
    {
        DateNullable DeletedDate { get; }

        bool IsDeleted() => DeletedDate.DateHasValue();

        void Delete();
    }
}
