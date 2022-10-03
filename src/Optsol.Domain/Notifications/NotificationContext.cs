using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Domain.Notifications
{
    public class NotificationContext
    {
        public NotificationCollection Notifications { get; private set; }

        public static NotificationContext Create() => 
            new()
            {
                Notifications = NotificationCollection.Create()
            };

        public void AddNotification(string key, string message) =>
            Notifications.Insert(Notification.Create(key, message));

        public void AddNotification(Notification notification) =>
            Notifications.Insert(notification);

        public void AddNotifications(params Notification[] notifications) =>
            Notifications.Insert(notifications);

        public void AddNotifications(IList<Notification> notifications) =>
            Notifications.Insert(notifications.ToArray());

        public void AddNotifications(ICollection<Notification> notifications) =>
            Notifications.Insert(notifications.ToArray());

        public void AddNotifications(ValidationResult validationResult) =>
            AddNotifications(validationResult.Errors.Select(error => Notification.Create(error.ErrorCode, error.ErrorMessage)).ToArray());

        public void ClearNotifications() => Notifications.Clear();
    }
}
