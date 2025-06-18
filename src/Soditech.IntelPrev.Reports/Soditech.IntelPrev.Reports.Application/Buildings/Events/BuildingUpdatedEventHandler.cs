using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.Buildings.Events;

public class BuildingUpdatedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<BuildingUpdatedEvent>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ILogger<BuildingCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<BuildingCreatedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(BuildingUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var building = await _buildingRepository.GetAsync(notification.Id, cancellationToken);

            if (building == null)
            {
                building = _mapper.Map<Building>(notification);
                await _buildingRepository.AddAsync(building, cancellationToken);
            }
            else
            {
                _mapper.Map(notification, building);
                await _buildingRepository.UpdateAsync(building, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating building");
        }
    }
}