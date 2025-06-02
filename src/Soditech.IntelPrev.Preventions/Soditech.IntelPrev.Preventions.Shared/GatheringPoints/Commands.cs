using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

public record GatheringPointResult
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public Guid BuildingId { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    
    public Guid? FloorId { get; set; }
    public int? FloorNumber { get; set; }
    
    public Guid? GeoLocationId { get; set; }
    
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

public record CreateGatheringPointCommand : IRequest<TResult<GatheringPointResult>>
{
    public string Name { get; set; } = string.Empty;
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
}

public record UpdateGatheringPointCommand : IRequest<TResult<GatheringPointResult>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
}

public record DeleteGatheringPointCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetGatheringPointQuery : IRequest<TResult<GatheringPointResult>>
{
    public Guid Id { get; set; }
}

public record GetGatheringPointsQuery : IRequest<TResult<IEnumerable<GatheringPointResult>>>;

public record GetGatheringPointsByBuildingQuery : IRequest<TResult<IEnumerable<GatheringPointResult>>>
{
    public Guid? BuildingId { get; set; }
    public Guid? FloorId { get; set; }
}


public class UpdateGatheringPointGeoLocationCommand : IRequest<Result>
{
    public Guid GatheringPointId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }

}



public record GetGatheringPointsCountQuery : IRequest<TResult<int>>;