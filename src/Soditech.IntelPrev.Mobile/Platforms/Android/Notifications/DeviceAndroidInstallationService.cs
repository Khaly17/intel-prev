using System;
using Android.Gms.Common;
using Android.Provider;
using Microsoft.Maui.ApplicationModel;
using Soditech.IntelPrev.Mobile.Services.Notifications;
using Soditech.IntelPrev.Notifications.Shared.Models;

namespace Soditech.IntelPrev.Mobile;

public class DeviceAndroidInstallationService : IDeviceInstallationService
{
    public string? Token { get; set; }

    public bool NotificationsSupported
        => GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Platform.AppContext) == ConnectionResult.Success;

    public string? GetDeviceId()
        => Settings.Secure.GetString(Platform.AppContext.ContentResolver, Settings.Secure.AndroidId);

    public DeviceInstallation GetDeviceInstallation(params string[] tags)
    {
        if (!NotificationsSupported)
            throw new ArgumentException(GetPlayServicesError());

        if (string.IsNullOrWhiteSpace(Token))
            throw new ArgumentException("Unable to resolve token for FCMv1.");

        var installation = new DeviceInstallation
        {
            InstallationId = GetDeviceId(),
            Platform = "fcmv1",
            PushChannel = Token,
            Tags = [.. tags]
        };

        return installation;
    }

    static string GetPlayServicesError()
    {
        var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Platform.AppContext);

        if (resultCode != ConnectionResult.Success)
            return GoogleApiAvailability.Instance.IsUserResolvableError(resultCode) ?
                GoogleApiAvailability.Instance.GetErrorString(resultCode) :
                "This device isn't supported.";

        return "An error occurred preventing the use of push notifications.";
    }
}