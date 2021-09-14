using Optsol.Components.Repository.Domain.Objects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntity : IEquatable<IEntity>
    {
        Key Id { get; }
    }
}
