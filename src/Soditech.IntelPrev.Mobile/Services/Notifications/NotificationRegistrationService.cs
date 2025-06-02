using System.Text.Json;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Notifications.Shared.Models;
using Soditech.IntelPrev.Notifications.Shared.NotificationHubs;
using Soditech.IntelPrev.Proxy;

namespace Soditech.IntelPrev.Mobile.Services.Notifications;

public class NotificationRegistrationService : INotificationRegistrationService
{
    const string RequestUrl = "api/notifications/installations";
    const string CachedDeviceTokenKey = "cached_device_token";
    const string CachedTagsKey = "cached_tags";

    IProxyService _proxyService;
    IDeviceInstallationService _deviceInstallationService;

    IProxyService ProxyService =>
        _proxyService ??= DependencyResolver.GetRequiredService<IProxyService>();

    IDeviceInstallationService DeviceInstallationService =>
        _deviceInstallationService ??= DependencyResolver.GetRequiredService<IDeviceInstallationService>();


    public async Task DeregisterDeviceAsync()
    {
        var cachedToken = await SecureStorage.GetAsync(CachedDeviceTokenKey)
            .ConfigureAwait(false);

        if (cachedToken == null)
            return;

        var deviceId = DeviceInstallationService?.GetDeviceId();

        if (string.IsNullOrWhiteSpace(deviceId))
            throw new Exception("Unable to resolve an ID for the device.");

        await ProxyService.DeleteAsync( $"{RequestUrl}/{deviceId}")
            .ConfigureAwait(false);

        SecureStorage.Remove(CachedDeviceTokenKey);
        SecureStorage.Remove(CachedTagsKey);
    }

    public async Task RegisterDeviceAsync(params string[] tags)
    {
        var deviceInstallation = DeviceInstallationService.GetDeviceInstallation(tags);

        var request = new UpdateInstallationRequest
        {
            DeviceInstallation = deviceInstallation
        };

        await ProxyService.PutAsync<DeviceInstallation>( RequestUrl, request)
            .ConfigureAwait(false);

        await SecureStorage.SetAsync(CachedDeviceTokenKey, deviceInstallation.PushChannel)
            .ConfigureAwait(false);

        await SecureStorage.SetAsync(CachedTagsKey, JsonSerializer.Serialize(tags));
    }

    public async Task RefreshRegistrationAsync()
    {
        var cachedToken = await SecureStorage.GetAsync(CachedDeviceTokenKey)
            .ConfigureAwait(false);

        var serializedTags = await SecureStorage.GetAsync(CachedTagsKey)
            .ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(cachedToken) ||
            string.IsNullOrWhiteSpace(serializedTags) ||
            string.IsNullOrWhiteSpace(_deviceInstallationService.Token) ||
            cachedToken == DeviceInstallationService.Token)
            return;

        var tags = JsonSerializer.Deserialize<string[]>(serializedTags);

        await RegisterDeviceAsync(tags);
    }

}