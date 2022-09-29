using Optsol.Domain.ValueObjects;

namespace Optsol.Domain.Entities
{
    public interface IEntityDeletable
    {
        DateNullable DeletedDate { get; }

        bool IsDeleted() => DeletedDate.DateHasValue();

        void Delete();
    }
}
