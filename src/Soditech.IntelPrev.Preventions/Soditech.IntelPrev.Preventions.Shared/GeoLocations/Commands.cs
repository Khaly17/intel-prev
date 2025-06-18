using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Enums;

namespace Soditech.IntelPrev.Prevensions.Shared.GeoLocations;

public class GeoLocationResult 
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public string Type { get; set; } = string.Empty;

    public Guid BuildingId { get; set; }
    public string BuildingName { get; set; } = string.Empty;


    public Guid? FloorId { get; set; }
    public int? FloorNumber { get; set; }

    public Guid? EquipmentId { get; set; }
    public string? EquipmentName { get; set; } 
    
    public Guid? GatheringPointId { get; set; }
    public string? GatheringPointName { get; set; } 


    public string? CreatorFullName { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public string? DeleterFullName { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public string? UpdaterFullName { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class AddGeoLocationCommand : IRequest<Result>
{
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
    public Guid? EquipmentId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public string Type { get; set; } = GeoLocationType.Extinguisher.ToString();

}

public class UpdateGeoLocationCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
    public Guid? EquipmentId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }

}


public class GetLocationsByTypeQuery : IRequest<TResult<IEnumerable<GeoLocationResult>>>
{
    public Guid? BuildingId { get; set; }
    public Guid? FloorId { get; set; }
    
    public string Type { get; set; } = GeoLocationType.Extinguisher.ToString();
}