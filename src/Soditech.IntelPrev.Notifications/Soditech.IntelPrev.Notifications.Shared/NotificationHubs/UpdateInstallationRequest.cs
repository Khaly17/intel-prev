using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Notifications.Shared.Models;

namespace Soditech.IntelPrev.Notifications.Shared.NotificationHubs;

public class UpdateInstallationRequest : IRequest<Result>
{
    public required DeviceInstallation DeviceInstallation { get; set; }
}