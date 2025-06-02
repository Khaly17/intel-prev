using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Preventions.Shared.Floors;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.Floors.Events;

public class FloorUpdatedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<FloorUpdatedEvent>
{
    private readonly IRepository<Floor> _floorRepository = serviceProvider.GetRequiredService<IRepository<Floor>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ILogger<FloorCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<FloorCreatedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(FloorUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var floor = _mapper.Map<Floor>(notification);
            await _floorRepository.UpdateAsync(floor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating building");
        }
    }
}