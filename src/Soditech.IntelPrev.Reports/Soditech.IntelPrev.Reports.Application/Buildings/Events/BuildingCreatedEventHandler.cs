using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Preventions.Shared.Buildings;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.Buildings.Events;

public class BuildingCreatedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<BuildingCreatedEvent>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ILogger<BuildingCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<BuildingCreatedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(BuildingCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var building = _mapper.Map<Building>(notification);
            await _buildingRepository.AddAsync(building, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating building");
        }
    }
}