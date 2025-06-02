using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.NotificationHubs.Application.Services;
using Soditech.IntelPrev.Notifications.Shared.NotificationHubs;

namespace Soditech.IntelPrev.NotificationHubs.Application.Installations.Commands;

public class DeleteInstallationCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteInstallationRequest, Result>
{
    private readonly INotificationService _notificationService =
        serviceProvider.GetRequiredService<INotificationService>();

    private readonly ILogger<DeleteInstallationCommandHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<DeleteInstallationCommandHandler>>();
    /// <inheritdoc />
    public async Task<Result> Handle(DeleteInstallationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _notificationService
                .DeleteInstallationByIdAsync(request.InstallationId, cancellationToken);

            if (!success)
            {
                return Result.Failure(new Error("500", "Cannot delete installation."));
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting installation for `{installationId}`", request.InstallationId);
            return Result.Failure(new Error("500", "Cannot delete installation."));
        }

        return Result.Success();
    }
}