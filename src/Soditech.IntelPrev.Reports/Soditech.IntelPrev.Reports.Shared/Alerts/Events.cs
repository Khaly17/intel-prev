using MediatR;

namespace Soditech.IntelPrev.Reports.Shared.Alerts;

public record AlertCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Guid BuildingId { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public Guid? FloorId { get; set; }
    public int? FloorNumber { get; set; }

    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record AlertUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }



    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
public record AlertDeletedEvent(Guid Id) : INotification;