using Soditech.IntelPrev.NotificationHubs.Application.Models;
using Soditech.IntelPrev.Notifications.Shared.Models;

namespace Soditech.IntelPrev.NotificationHubs.Application.Services;

public interface INotificationService
{
    Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken cancellationToken);
    Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken cancellationToken);
    Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken cancellationToken);
}