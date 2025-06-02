namespace Soditech.IntelPrev.Notifications.Shared;

public class NotificationRoutes
{
    public class Installation
    {
        public const string Update = "api/notifications/installations";
        public const string Delete = "api/notifications/installations/{installationId}";
        public const string Request = "api/notifications/request";
    }

    public class Requests
    {
        public const string Request = "api/notifications/request";
    }
}