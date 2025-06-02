using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Preventions.Shared.Equipments;

public record EquipmentResult
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTimeOffset LastInspectionDate { get; set; }
    public DateTimeOffset NextInspectionDate { get; set; }
    
    public Guid BuildingId { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    
    public Guid? FloorId { get; set; }
    public int? FloorNumber { get; set; }
    
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

public record CreateEquipmentCommand : IRequest<TResult<EquipmentResult>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTimeOffset LastInspectionDate { get; set; }
    public DateTimeOffset NextInspectionDate { get; set; }
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
}

public record UpdateEquipmentCommand : IRequest<TResult<EquipmentResult>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTimeOffset LastInspectionDate { get; set; }
    public DateTimeOffset NextInspectionDate { get; set; }
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
}

public record DeleteEquipmentCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetEquipmentQuery : IRequest<TResult<EquipmentResult>>
{
    public Guid Id { get; set; }
}

public record GetEquipmentsQuery : IRequest<TResult<IEnumerable<EquipmentResult>>>;

public record GetEquipmentsByTypeQuery : IRequest<TResult<IEnumerable<EquipmentResult>>>
{
    public string Type { get; set; } = string.Empty;
}

public class UpdateEquipmentGeoLocationCommand : IRequest<Result>
{
    public Guid EquipmentId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }

}



public record GetEquipmentsCountQuery : IRequest<TResult<int>>;