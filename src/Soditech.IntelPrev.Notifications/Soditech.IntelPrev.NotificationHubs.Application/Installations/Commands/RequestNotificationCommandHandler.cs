using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.NotificationHubs.Application.Services;
using Soditech.IntelPrev.Notifications.Shared.NotificationHubs;

namespace Soditech.IntelPrev.NotificationHubs.Application.Installations.Commands;

public class RequestNotificationCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<RequestPushRequest, Result>
{
    private readonly INotificationService _notificationService =
        serviceProvider.GetRequiredService<INotificationService>();

    private readonly ILogger<RequestNotificationCommandHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<RequestNotificationCommandHandler>>();
    /// <inheritdoc />
    public async Task<Result> Handle(RequestPushRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if ((request.NotificationRequest.Silent &&
                 string.IsNullOrWhiteSpace(request.NotificationRequest.Action)) ||
                (!request.NotificationRequest.Silent &&
                 string.IsNullOrWhiteSpace(request.NotificationRequest.Text)))
            {
                return Result.Failure(new Error("400", "Cannot request notification"));
            }

            var success = await _notificationService
                .RequestNotificationAsync(request.NotificationRequest, cancellationToken);

            if (!success)
            {
                return Result.Failure(new Error("500", "Cannot request notification."));
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while requesting notification.");
            return Result.Failure(new Error("500", "Cannot request notification."));
        }

        return Result.Success();
    }
}