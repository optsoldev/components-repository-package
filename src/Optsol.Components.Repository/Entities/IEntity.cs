using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntity : IEquatable<IEntity>
    {
        Guid Id { get; }
    }
}
