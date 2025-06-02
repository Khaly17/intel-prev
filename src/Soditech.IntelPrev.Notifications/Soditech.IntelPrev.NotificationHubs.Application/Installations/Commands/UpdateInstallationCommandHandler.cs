using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.NotificationHubs.Application.Services;
using Soditech.IntelPrev.Notifications.Shared.NotificationHubs;

namespace Soditech.IntelPrev.NotificationHubs.Application.Installations.Commands;

public class UpdateInstallationCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateInstallationRequest, Result>
{
    private readonly INotificationService _notificationService =
        serviceProvider.GetRequiredService<INotificationService>();

    private readonly ILogger<UpdateInstallationCommandHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<UpdateInstallationCommandHandler>>();
    /// <inheritdoc />
    public async Task<Result> Handle(UpdateInstallationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _notificationService
                .CreateOrUpdateInstallationAsync(request.DeviceInstallation, cancellationToken);

            if (!success)
            {
                return Result.Failure(new Error("500", "Cannot create/update installation."));
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating/updating installation for `{installationId}`", request.DeviceInstallation);
            return Result.Failure(new Error("500", "Cannot create/update installation."));
        }

        return Result.Success();
    }
}