using Soditech.IntelPrev.Notifications.Shared.Models;

namespace Soditech.IntelPrev.Mobile.Services.Notifications;

public interface IDeviceInstallationService
{
    string Token { get; set; }
    bool NotificationsSupported { get; }
    string GetDeviceId();
    DeviceInstallation GetDeviceInstallation(params string[] tags);
}