using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Optsol.Components.Repository.Domain.Notifications
{
    public class NotificationCollection
    {
        private readonly IList<Notification> notifications;

        public NotificationCollection()
        {
            notifications = new List<Notification>();
        }

        public int Total => notifications.Count;

        public bool HasNotifications => notifications.Any();

        public IReadOnlyCollection<Notification> GetNotifications() =>
            new ReadOnlyCollection<Notification>(this.notifications);

        public void Insert(Notification notification) =>
            notifications.Add(notification);

        public void Insert(params Notification[] notifications) =>
            (this.notifications as List<Notification>).AddRange(notifications);

        public void Clear() => notifications.Clear();

        public void Delete(Notification notification) =>
            notifications.Remove(notification);

        public Notification FindByKey(string key) => 
            FindByKeys(key).FirstOrDefault();

        public IEnumerable<Notification> FindByKeys(params string[] Keys) =>
            notifications.Where(notification => Keys.Contains(notification.Key));
        
        public static NotificationCollection Create() => new();
    }
}
