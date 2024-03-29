﻿using FluentValidation.Results;

namespace Optsol.Domain.Notifications
{
    public interface INotification
    {
        bool Valid { get { return ValidationResult.IsValid; } }

        bool Invalid { get { return Valid is false; } }

        ValidationResult ValidationResult { get; }

        void Validate();
    }
}
