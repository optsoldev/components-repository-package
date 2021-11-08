using Optsol.Components.Repository.Domain.Exceptions;

namespace Optsol.Components.Repository.Domain.Notifications
{
    public class Notification
    {
        public string Key { get; protected set; }

        public string Message { get; protected set; }

        public static Notification Create(string key, string message)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new NotificationException($"{nameof(key)} is null");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new NotificationException($"{nameof(message)} is null");
            }

            var notification = new Notification
            {
                Key = key,
                Message = message
            };

            return notification;

        }
    }
}
