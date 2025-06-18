using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.Enums;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Equipments.Commands;

public class UpdateEquipmentGeoLocationCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateEquipmentGeoLocationCommand, Result>
{
    private readonly IRepository<Equipment> _equipmentRepository = serviceProvider.GetRequiredService<IRepository<Equipment>>();
    private readonly IRepository<GeoLocation> _geoLocationRepository = serviceProvider.GetRequiredService<IRepository<GeoLocation>>();
    private readonly ILogger<UpdateEquipmentGeoLocationCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateEquipmentGeoLocationCommandHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<Result> Handle(UpdateEquipmentGeoLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var equipment = await _equipmentRepository.GetAsync(request.EquipmentId, cancellationToken);

            if (equipment == null)
            {
                return Result.Failure<EquipmentResult>(new Error("404", "Equipment not found"));
            }

            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure(new Error("400", "cannot update geoLocation without a tenant"));
            }

            var type = equipment.Type.ToGeoLocationType();

            if (equipment is { GeoLocationId: not null, GeoLocation: not null } )
            {
                equipment.GeoLocation.Latitude = request.Latitude;
                equipment.GeoLocation.Longitude = request.Longitude;
                equipment.GeoLocation.Altitude = request.Altitude;
                equipment.GeoLocation.Type = type;

                await _geoLocationRepository.UpdateAsync(equipment.GeoLocation, cancellationToken);
            }
            else
            {
                var geoLocation = new GeoLocation
                {
                    TenantId = equipment.TenantId,
                    BuildingId = equipment.BuildingId,
                    FloorId = equipment.FloorId,
                    EquipmentId = equipment.Id,
                    CreatorId = _session.UserId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Altitude = request.Altitude,
                    Type = type
                };

                await _geoLocationRepository.AddAsync(geoLocation, cancellationToken);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot update geoLocation");
        }

        return Result.Failure(new Error("500", "Error while updating geoLocation"));
    }
}