using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Shared.Buildings;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Application.Buildings.Commands;


public class CreateBuildingCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateBuildingCommand, TResult<BuildingResult>>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly ILogger<CreateBuildingCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateBuildingCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<BuildingResult>> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<BuildingResult>(new Error("400", "cannot create building without a tenant"));
            }
            var building = _mapper.Map<Building>(request);
            building.TenantId = _session.TenantId.Value;
            
            building.CreatorId = _session.UserId;
            building.CreatedAt = DateTimeOffset.UtcNow;
            
            //generate a unique code for the building, to allow equipment to be linked to it
            building.Id = Guid.NewGuid();

            foreach (var floor in building.Floors)
            {
                floor.CreatorId = building.CreatorId;
                floor.CreatedAt = building.CreatedAt;
                floor.TenantId = building.TenantId;
                floor.BuildingId = building.Id;
                
                foreach (var equipment in floor.Equipments)
                {
                    equipment.CreatorId = building.CreatorId;
                    equipment.CreatedAt = building.CreatedAt;
                    equipment.TenantId = building.TenantId;
                    equipment.BuildingId = building.Id;
                }
            }

            foreach (var equipment in building.Equipments)
            {
                equipment.CreatorId = building.CreatorId;
                equipment.CreatedAt = building.CreatedAt;
                equipment.TenantId = building.TenantId;
                equipment.BuildingId = building.Id;
            }

            await _buildingRepository.AddAsync(building, cancellationToken);

            await _publisher.Publish(_mapper.Map<BuildingCreatedEvent>(building), cancellationToken);
            
            return Result.Success(_mapper.Map<BuildingResult>(building));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create building");
        }
        
        return Result.Failure<BuildingResult>(new Error("500", "Error while creating building"));
    }   
}