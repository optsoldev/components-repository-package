using Optsol.Components.Repository.Domain.Exceptions;

namespace Optsol.Components.Repository.Domain.Notifications
{
    public class Notification
    {
        public string Key { get; private set; }

        public string Message { get; private set; }

        public Notification(string key, string message)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new NotificationException($"{nameof(key)} is null");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new NotificationException($"{nameof(message)} is null");
            }


            Key = key;
            Message = message;
        }
    }
}
