using Foundation;
using Soditech.IntelPrev.Mobile.Services.Notifications;
using Soditech.IntelPrev.Notifications.Shared.Models;
using System.Text;
using UIKit;

namespace Soditech.IntelPrev.Mobile;

public class DeviceiOsInstallationService : IDeviceInstallationService
{
    const int SupportedVersionMajor = 13;
    const int SupportedVersionMinor = 0;

    public string Token { get; set; }

    public bool NotificationsSupported =>
        UIDevice.CurrentDevice.CheckSystemVersion(SupportedVersionMajor, SupportedVersionMinor);

    public string GetDeviceId() =>
        UIDevice.CurrentDevice.IdentifierForVendor.ToString();

    public DeviceInstallation GetDeviceInstallation(params string[] tags)
    {
        if (!NotificationsSupported)
            throw new Exception(GetNotificationsSupportError());

        if (string.IsNullOrWhiteSpace(Token))
            throw new Exception("Unable to resolve token for APNS");

        var installation = new DeviceInstallation
        {
            InstallationId = GetDeviceId(),
            Platform = "apns",
            PushChannel = Token,
            Tags = [..tags]
        };


        return installation;
    }

    string GetNotificationsSupportError()
    {
        if (!NotificationsSupported)
            return "This app only supports notifications on iOS {SupportedVersionMajor}.{SupportedVersionMinor} and above. You are running {UIDevice.CurrentDevice.SystemVersion}.";

        if (string.IsNullOrWhiteSpace(Token))
            return "This app can support notifications but you must enable this in your settings.";

        return "An error occurred preventing the use of push notifications";
    }
}

internal static class NSDataExtensions
{
    internal static string ToHexString(this NSData data)
    {
        var bytes = data.ToArray();

        if (bytes == null)
            return null;

        var sb = new StringBuilder(bytes.Length * 2);

        foreach (var b in bytes)
            sb.AppendFormat("{0:x2}", b);

        return sb.ToString().ToUpperInvariant();
    }
}