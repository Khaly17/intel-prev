using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Preventions.Shared.Floors;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.Floors.Events;

public class FloorDeletedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<FloorDeletedEvent>
{
    private readonly IRepository<Floor> _floorRepository = serviceProvider.GetRequiredService<IRepository<Floor>>();
    private readonly ILogger<FloorDeletedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<FloorDeletedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(FloorDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var floor = await  _floorRepository.GetAsync(notification.Id, cancellationToken);
            if (floor != null)
            {
                await _floorRepository.DeleteAsync(floor, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting floor");
        }
    }
}