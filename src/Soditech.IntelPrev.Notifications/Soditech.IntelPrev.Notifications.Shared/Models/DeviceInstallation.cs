namespace Soditech.IntelPrev.Notifications.Shared.Models;

public class DeviceInstallation
{
    public required string InstallationId { get; set; }

    public required string Platform { get; set; }

    public required string PushChannel { get; set; }

    public IList<string> Tags { get; set; } = [];
}