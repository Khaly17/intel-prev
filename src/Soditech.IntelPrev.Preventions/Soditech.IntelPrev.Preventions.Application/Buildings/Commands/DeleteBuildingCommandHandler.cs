using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Buildings.Commands;

public class DeleteBuildingCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteBuildingCommand, Result>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly ILogger<DeleteBuildingCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteBuildingCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var building = await _buildingRepository.GetAsync(request.Id, cancellationToken);
            if (building == null)
            {
                return Result.Failure<BuildingResult>(new Error("404", "Building not found"));
            }
            
            await _buildingRepository.DeleteAsync(building, cancellationToken);
            
            await _publisher.Publish(new BuildingDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting building");

            return Result.Failure(new Error("500", "Error while deleting building"));
        }
    }   
}