using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.Shared.Floors;

public record FloorResult
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }

    public int Number { get; set; }
    public Guid BuildingId { get; set; }
    public string BuildingName { get; set; } = string.Empty;

    public string? CreatorFullName { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public string? DeleterFullName { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public string? UpdaterFullName { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public List<EquipmentResult> Equipments { get; set; } = default!;

}

public record CreateFloorCommand : IRequest<Result>
{
    public int Number { get; set; }
    public Guid BuildingId { get; set; }
    public List<CreateEquipmentCommand> Equipments { get; set; } = default!;

}

public record UpdateFloorCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Guid BuildingId { get; set; }
    public List<UpdateEquipmentCommand> Equipments { get; set; } = default!;

}