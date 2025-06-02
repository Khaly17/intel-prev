using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.Application.Alerts.Commands;

public class DeleteAlertCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteAlertCommand, Result>
{
    private readonly IRepository<Alert> _alertRepository = serviceProvider.GetRequiredService<IRepository<Alert>>();
    private readonly ILogger<DeleteAlertCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteAlertCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteAlertCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var alert = await _alertRepository.GetAsync(request.Id, cancellationToken);
            if (alert == null)
            {
                return Result.Failure<AlertResult>(new Error("404", "Alert not found"));
            }
            
            await _alertRepository.DeleteAsync(alert, cancellationToken);
            
            await _publisher.Publish(new AlertDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting alert");

            return Result.Failure(new Error("500", "Error while deleting alert"));
        }
    }   
}