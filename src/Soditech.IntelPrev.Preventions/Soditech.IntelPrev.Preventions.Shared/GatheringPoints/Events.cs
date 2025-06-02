using MediatR;

namespace Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

public record GatheringPointCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
    public Guid? GeoLocationId { get; set; }

    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public string? DeleterFullName { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record GatheringPointUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
    public Guid? GeoLocationId { get; set; }

    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record GatheringPointDeletedEvent(Guid Id) : INotification ;