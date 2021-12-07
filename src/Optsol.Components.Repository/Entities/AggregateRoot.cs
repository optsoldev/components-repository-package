﻿using Optsol.Components.Repository.Domain.ValueObjects;
using System;

namespace Optsol.Components.Repository.Domain.Entities
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        public DateValue CreateDate { get; private set; }

        public AggregateRoot()
        {
            Key = KeyGuid.Create(Guid.NewGuid());
            CreateDate = DateValue.Create().SetDateValueWithDateOfNow();
        }

        public override bool Equals(object obj) => Equals(obj as IAggregateRoot);

        public override int GetHashCode() => $"{GetType()}{Key.GetHashCode()}".GetHashCode();
    }
}
