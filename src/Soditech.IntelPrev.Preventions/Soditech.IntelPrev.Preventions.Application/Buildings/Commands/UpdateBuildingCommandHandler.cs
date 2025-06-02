using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Buildings;

namespace Soditech.IntelPrev.Preventions.Application.Buildings.Commands;

public class UpdateBuildingCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateBuildingCommand, TResult<BuildingResult>>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly ILogger<UpdateBuildingCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateBuildingCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<BuildingResult>> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var building = await _buildingRepository.GetAsync(request.Id, cancellationToken);
            if (building == null)
            {
                return Result.Failure<BuildingResult>(new Error("404", "Building not found"));
            }
            
            _mapper.Map(request, building);
            
            building.UpdaterId = _session.UserId;
            building.UpdatedAt = DateTimeOffset.UtcNow;
            
            foreach (var floor in building.Floors)
            {
                floor.TenantId = building.TenantId;
                floor.BuildingId = building.Id;
                floor.CreatorId = building.CreatorId;
                floor.CreatedAt = building.CreatedAt;
                floor.UpdaterId = building.UpdaterId;
                floor.UpdatedAt = building.UpdatedAt;
                
                foreach (var equipment in floor.Equipments)
                {
                    equipment.TenantId = building.TenantId;
                    equipment.BuildingId = building.Id;
                    equipment.CreatorId = building.CreatorId;
                    equipment.CreatedAt = building.CreatedAt;
                    equipment.UpdaterId = building.UpdaterId;
                    equipment.UpdatedAt = building.UpdatedAt;
                }
            }

            foreach (var equipment in building.Equipments)
            {
                equipment.TenantId = building.TenantId;
                equipment.BuildingId = building.Id;
                equipment.CreatorId = building.CreatorId;
                equipment.CreatedAt = building.CreatedAt;
                equipment.UpdaterId = building.UpdaterId;
                equipment.UpdatedAt = building.UpdatedAt;
            }

            
            await _buildingRepository.UpdateAsync(building, cancellationToken);
            await _publisher.Publish(_mapper.Map<BuildingUpdatedEvent>(building), cancellationToken);

            
            return Result.Success(_mapper.Map<BuildingResult>(building));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating building");

            return Result.Failure<BuildingResult>(new Error("500", "Error while updating building"));
        }
    }   
}