using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Repository.Domain.Notifications
{
    public class NotificationContext
    {
        public NotificationCollection Notifications { get; private set; }

        public NotificationContext()
        {
            Notifications = NotificationCollection.Create();
        }

        public void AddNotification(string key, string message) =>
            Notifications.Insert(new Notification(key, message));

        public void AddNotification(Notification notification) =>
            Notifications.Insert(notification);

        public void AddNotification(params Notification[] notifications) =>
            Notifications.Insert(notifications);

        public void AddNotifications(IList<Notification> notifications) =>
            Notifications.Insert(notifications.ToArray());

        public void AddNotifications(ICollection<Notification> notifications) =>
            Notifications.Insert(notifications.ToArray());

        public void AddNotifications(ValidationResult validationResult) =>
            AddNotification(validationResult.Errors.Select(error => new Notification(error.ErrorCode, error.ErrorMessage)).ToArray());

        public void ClearNotifications() => Notifications.Clear();
    }
}
