using Optsol.Components.Repository.Domain.Objects;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntityDeletable
    {
        DateNulable DeletedDate { get; }

        bool IsDeleted() => DeletedDate.DateHasValue();

        void Delete();
    }
}
