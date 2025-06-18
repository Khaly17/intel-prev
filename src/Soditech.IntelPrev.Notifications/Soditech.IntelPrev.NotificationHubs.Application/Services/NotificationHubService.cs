using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Soditech.IntelPrev.NotificationHubs.Application.Models;
using Soditech.IntelPrev.Notifications.Shared.Models;

namespace Soditech.IntelPrev.NotificationHubs.Application.Services;

public class NotificationHubService(IOptions<NotificationHubOptions> options, ILogger<NotificationHubService> logger)
    : INotificationService
{
    private readonly NotificationHubClient _hub = NotificationHubClient.CreateClientFromConnectionString(options.Value.ConnectionString, options.Value.Name);

    private readonly Dictionary<string, NotificationPlatform> _installationPlatform = new()
    {
        { nameof(NotificationPlatform.Apns).ToLower(), NotificationPlatform.Apns },
        { nameof(NotificationPlatform.FcmV1).ToLower(), NotificationPlatform.FcmV1 }
    };

    public async Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(deviceInstallation.InstallationId) ||
            string.IsNullOrWhiteSpace(deviceInstallation.Platform) ||
            string.IsNullOrWhiteSpace(deviceInstallation.PushChannel))
            return false;

        var installation = new Installation()
        {
            InstallationId = deviceInstallation.InstallationId,
            PushChannel = deviceInstallation.PushChannel,
            Tags = deviceInstallation.Tags
        };

        if (_installationPlatform.TryGetValue(deviceInstallation.Platform, out var platform))
            installation.Platform = platform;
        else
            return false;

        try
        {
            await _hub.CreateOrUpdateInstallationAsync(installation, cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(installationId))
            return false;

        try
        {
            await _hub.DeleteInstallationAsync(installationId, cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken cancellationToken)
    {
        if ((notificationRequest.Silent && string.IsNullOrWhiteSpace(notificationRequest.Action)) ||
            (!notificationRequest.Silent && string.IsNullOrWhiteSpace(notificationRequest.Text)))
            return false;

        var androidPushTemplate = notificationRequest.Silent ?
            PushTemplates.Silent.Android :
            PushTemplates.Generic.Android;

        var iOSPushTemplate = notificationRequest.Silent ?
            PushTemplates.Silent.iOS :
            PushTemplates.Generic.iOS;

        var androidPayload = PrepareNotificationPayload(
            androidPushTemplate,
            notificationRequest.Title,
            notificationRequest.Text,
            notificationRequest.Action);

        var iOSPayload = PrepareNotificationPayload(
            iOSPushTemplate,
            notificationRequest.Title,
            notificationRequest.Text,
            notificationRequest.Action);

        try
        {
            switch (notificationRequest.Tags.Count)
            {
                case 0:
                    // This will broadcast to all users registered in the notification hub
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, cancellationToken);
                    break;
                case <= 20:
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, notificationRequest.Tags, cancellationToken);
                    break;
                default:
                {
                    var notificationTasks = notificationRequest.Tags
                        .Select((value, index) => (value, index))
                        .GroupBy(g => g.index / 20, i => i.value)
                        .Select(tags => SendPlatformNotificationsAsync(androidPayload, iOSPayload, tags, cancellationToken));

                    await Task.WhenAll(notificationTasks);
                    break;
                }
            }

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error sending notification");
            return false;
        }
    }

    static string PrepareNotificationPayload(string template,string title ,string text, string action) => template
        .Replace("$(alertTitle)", title, StringComparison.InvariantCulture)
        .Replace("$(alertMessage)", text, StringComparison.InvariantCulture)
        .Replace("$(alertAction)", action, StringComparison.InvariantCulture);

    Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, CancellationToken token)
    {
        var sendTasks = new Task[]
        {
            _hub.SendFcmV1NativeNotificationAsync(androidPayload, token),
            //TODO: uncomment this to allow ANP
            //_hub.SendAppleNativeNotificationAsync(iOSPayload, token)
        };

        return Task.WhenAll(sendTasks);
    }

    Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, IEnumerable<string> tags, CancellationToken token)
    {
        var sendTasks = new Task[]
        {
            _hub.SendFcmV1NativeNotificationAsync(androidPayload, tags, token),
            _hub.SendAppleNativeNotificationAsync(iOSPayload, tags, token)
        };

        return Task.WhenAll(sendTasks);
    }
}