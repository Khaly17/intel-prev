using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Notifications.Shared.Models;
using Soditech.IntelPrev.Notifications.Shared.NotificationHubs;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.NotificationHubs.Application.Reports.Events;

public class ReportCreatedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<ReportCreatedEvent>
{
    private readonly ILogger<ReportCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<ReportCreatedEventHandler>>();
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

    /// <inheritdoc />
    public async Task Handle(ReportCreatedEvent notification, CancellationToken cancellationToken)
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
            _logger.LogError(ex, "Error while pushing notification for report creation.");
        }
    }
}