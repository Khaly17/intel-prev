using MediatR;

namespace Soditech.IntelPrev.Preventions.Shared.Floors;

public record FloorCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }

    public int Number { get; init; }
    public Guid BuildingId { get; set; }

    public Guid TenantId { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record FloorUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }

    public int Number { get; init; }
    public Guid BuildingId { get; set; }

    public Guid TenantId { get; set; }

    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record FloorDeletedEvent(Guid Id) : INotification;