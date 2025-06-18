using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Application.Buildings.Events;

public class BuildingDeletedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<BuildingDeletedEvent>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly ILogger<BuildingDeletedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<BuildingDeletedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(BuildingDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var building = await  _buildingRepository.GetAsync(notification.Id, cancellationToken);
            if (building != null)
            {
                await _buildingRepository.DeleteAsync(building, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting building");
        }
    }
}