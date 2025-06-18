using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Notifications.Shared.Models;
using Soditech.IntelPrev.Notifications.Shared.NotificationHubs;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.NotificationHubs.Application.Alerts.Events;

public class AlertCreatedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<AlertCreatedEvent>
{
    private readonly ILogger<AlertCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<AlertCreatedEventHandler>>();
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

    /// <inheritdoc />
    public async Task Handle(AlertCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var notificationRequest = new RequestPushRequest
            {
                NotificationRequest = new NotificationRequest()
                {
                    Silent = false,
                    Title = notification.Title,
                    Text = notification.Description,
                    //Tags = notification.
                }
            };

            await _mediator.Send(notificationRequest, cancellationToken);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while pushing notification for alert creation.");
        }
    }
}