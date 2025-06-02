namespace Soditech.IntelPrev.NotificationHubs.Application.Models;

/// <summary>
/// The PushTemplates class contains tokenized notification payloads for generic and silent Push notifications.
/// </summary>
public class PushTemplates
{
    public class Generic
    {
        public const string Android = "{ \"message\" : { \"notification\" : { \"title\" : \"$(alertTitle)\", \"body\" : \"$(alertMessage)\"}, \"data\" : { \"action\" : \"$(alertAction)\" } } }";
        public const string iOS = "{ \"aps\" : {\"alert\" : \"$(alertMessage)\"}, \"action\" : \"$(alertAction)\" }";
    }

    public class Silent
    {
        public const string Android = "{ \"message\" : { \"data\" : {\"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\"} } }";
        public const string iOS = "{ \"aps\" : {\"content-available\" : 1, \"apns-priority\": 5, \"sound\" : \"\", \"badge\" : 0}, \"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\" }";
    }
}