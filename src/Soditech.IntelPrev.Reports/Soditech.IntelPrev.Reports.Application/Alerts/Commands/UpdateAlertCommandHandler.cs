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
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.Application.Alerts.Commands;

public class UpdateAlertCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateAlertCommand, TResult<AlertResult>>
{
    private readonly IRepository<Alert> _alertRepository = serviceProvider.GetRequiredService<IRepository<Alert>>();
    private readonly ILogger<UpdateAlertCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateAlertCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<AlertResult>> Handle(UpdateAlertCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var alert = await _alertRepository.GetAsync(request.Id, cancellationToken);
            if (alert == null)
            {
                return Result.Failure<AlertResult>(new Error("404", "Alert not found"));
            }
            
            _mapper.Map(request, alert);
            
            alert.UpdaterId = _session.UserId;
            alert.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _alertRepository.UpdateAsync(alert, cancellationToken);
            await _publisher.Publish(_mapper.Map<AlertUpdatedEvent>(alert), cancellationToken);

            
            return Result.Success(_mapper.Map<AlertResult>(alert));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating alert");

            return Result.Failure<AlertResult>(new Error("500", "Error while updating alert"));
        }
    }   
}