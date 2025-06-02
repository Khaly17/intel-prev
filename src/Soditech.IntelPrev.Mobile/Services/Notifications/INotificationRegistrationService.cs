namespace Soditech.IntelPrev.Mobile.Services.Notifications;

public interface INotificationRegistrationService
{
    Task DeregisterDeviceAsync();
    Task RegisterDeviceAsync(params string[] tags);
    Task RefreshRegistrationAsync();
}