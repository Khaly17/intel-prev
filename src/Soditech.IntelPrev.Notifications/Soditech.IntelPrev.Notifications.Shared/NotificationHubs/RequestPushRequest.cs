using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Notifications.Shared.Models;

namespace Soditech.IntelPrev.Notifications.Shared.NotificationHubs;

public class RequestPushRequest : IRequest<Result>
{
    public required NotificationRequest NotificationRequest { get; set; }
}