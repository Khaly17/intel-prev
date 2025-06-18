using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Shared.Alerts;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Microsoft.AspNetCore.SignalR;

namespace Soditech.IntelPrev.Reports.Application.Alerts.Commands;


public class CreateAlertCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateAlertCommand, Result>
{
    private readonly IRepository<Alert> _alertRepository = serviceProvider.GetRequiredService<IRepository<Alert>>();
    private readonly ILogger<CreateAlertCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateAlertCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly IHubContext<NotificationsHub, INotificationClient> _context = serviceProvider.GetRequiredService<IHubContext<NotificationsHub, INotificationClient>>();

    public async Task<Result> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure(new Error("400", "cannot create Alert without a tenant"));
            }
            var alert = _mapper.Map<Alert>(request);
            alert.TenantId = _session.TenantId.Value;
            
            alert.CreatorId = _session.UserId;
            alert.CreatedAt = DateTimeOffset.UtcNow;

            await _alertRepository.AddAsync(alert, cancellationToken);

            //TODO: 1. send email/sms to all admins of the tenant

            //TODO: 2. send app notification/sms to all users of the corresponding building

            await _publisher.Publish(_mapper.Map<AlertCreatedEvent>(alert), cancellationToken);


            await _context.Clients.All.ReceiveNotification("Une nouvelle alerte a été créé.");            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create Alert");
        }
        
        return Result.Failure(new Error("500", "Error while creating Alert"));
    }   
}