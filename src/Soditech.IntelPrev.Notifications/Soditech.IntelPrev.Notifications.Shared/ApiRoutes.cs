namespace Soditech.IntelPrev.Notifications.Shared;

public class NotificationRoutes
{
    public static class Installation
    {
        public const string Update = "api/notifications/installations";
        public const string Delete = "api/notifications/installations/{installationId}";
        public const string Request = "api/notifications/request";
    }

    public static class Requests
    {
        public const string Request = "api/notifications/request";
    }
}