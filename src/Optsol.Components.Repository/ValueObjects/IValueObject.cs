using System;
using System.Collections.Generic;

namespace Optsol.Components.Repository.Domain.ValueObjects
{
    public interface IValueObject: IEquatable<IValueObject>
    {
        IEnumerable<object> GetEqualityComponents();
    }
}
