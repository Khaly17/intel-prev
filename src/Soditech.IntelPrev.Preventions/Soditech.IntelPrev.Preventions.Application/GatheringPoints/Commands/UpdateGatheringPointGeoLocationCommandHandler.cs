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
using Soditech.IntelPrev.Prevensions.Shared.GatheringPoints;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.GatheringPoints.Commands;

public class UpdateGatheringPointGeoLocationCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateGatheringPointGeoLocationCommand, Result>
{
    private readonly IRepository<GatheringPoint> _gatheringPointRepository = serviceProvider.GetRequiredService<IRepository<GatheringPoint>>();
    private readonly IRepository<GeoLocation> _geoLocationRepository = serviceProvider.GetRequiredService<IRepository<GeoLocation>>();
    private readonly ILogger<UpdateGatheringPointGeoLocationCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateGatheringPointGeoLocationCommandHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<Result> Handle(UpdateGatheringPointGeoLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var gatheringPoint = await _gatheringPointRepository.GetAsync(request.GatheringPointId, cancellationToken);

            if (gatheringPoint == null)
            {
                return Result.Failure<GatheringPointResult>(new Error("404", "GatheringPoint not found"));
            }

            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure(new Error("400", "cannot update geoLocation without a tenant"));
            }


            if (gatheringPoint is { GeoLocationId: not null, GeoLocation: not null } )
            {
                gatheringPoint.GeoLocation.Latitude = request.Latitude;
                gatheringPoint.GeoLocation.Longitude = request.Longitude;
                gatheringPoint.GeoLocation.Altitude = request.Altitude;
                gatheringPoint.GeoLocation.Type = GeoLocationType.GatheringPoint;

                await _geoLocationRepository.UpdateAsync(gatheringPoint.GeoLocation, cancellationToken);
            }
            else
            {
                var geoLocation = new GeoLocation
                {
                    TenantId = gatheringPoint.TenantId,
                    BuildingId = gatheringPoint.BuildingId,
                    FloorId = gatheringPoint.FloorId,
                    GatheringPointId = gatheringPoint.Id,
                    CreatorId = _session.UserId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Altitude = request.Altitude,
                    Type = GeoLocationType.GatheringPoint
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