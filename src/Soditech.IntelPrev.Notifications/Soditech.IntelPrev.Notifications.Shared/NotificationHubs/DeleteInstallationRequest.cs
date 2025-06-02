using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Notifications.Shared.NotificationHubs;

public class DeleteInstallationRequest : IRequest<Result>
{
    public required string InstallationId { get; set; }
}