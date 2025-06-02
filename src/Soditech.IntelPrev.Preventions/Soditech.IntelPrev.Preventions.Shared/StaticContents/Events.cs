using MediatR;

namespace Soditech.IntelPrev.Preventions.Shared.StaticContents;

public record StaticContentCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Key { get; set; } = string.Empty; 
    public string Title { get; set; } = string.Empty; 
    public string Content { get; set; } = string.Empty; 
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record StaticContentUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Key { get; set; } = string.Empty; 
    public string Title { get; set; } = string.Empty; 
    public string Content { get; set; } = string.Empty; 
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record StaticContentDeletedEvent(Guid Id) : INotification;