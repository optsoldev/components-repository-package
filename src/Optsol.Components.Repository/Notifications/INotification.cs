using FluentValidation.Results;

namespace Optsol.Components.Repository.Domain.Notifications
{
    public interface INotification
    {
        bool Valid { get; }

        bool Invalid { get; }

        ValidationResult ValidationResult { get; }
    }
}
