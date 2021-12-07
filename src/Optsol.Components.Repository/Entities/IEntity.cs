using Optsol.Components.Repository.Domain.ValueObjects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public interface IEntity : IEquatable<IEntity>
    {
        KeyGuid Key { get; }
    }
}
