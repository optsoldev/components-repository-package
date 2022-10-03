using System;

namespace Optsol.Domain.Entities
{
    public interface IEntity : IEquatable<IEntity>
    {
        Guid Id { get; }
    }
}
