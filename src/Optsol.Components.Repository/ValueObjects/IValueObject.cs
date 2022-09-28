using System;
using System.Collections.Generic;

namespace Optsol.Domain.ValueObjects
{
    public interface IValueObject: IEquatable<IValueObject>
    {
        IEnumerable<object> GetEqualityComponents();
    }
}
