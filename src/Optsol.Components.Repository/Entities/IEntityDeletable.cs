using Optsol.Components.Repository.Domain.Objects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntityDeletable
    {
        DateNullable DeletedDate { get; }

        bool IsDeleted() => DeletedDate.DateHasValue();

        void Delete();
    }
}
