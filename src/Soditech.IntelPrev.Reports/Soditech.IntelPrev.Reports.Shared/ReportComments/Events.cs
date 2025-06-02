using MediatR;

namespace Soditech.IntelPrev.Reports.Shared.ReportComments;

public record ReportCommentCreatedEvent : INotification
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    public Guid ReportId { get; set; }

    
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record ReportCommentUpdatedEvent : INotification
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    public Guid ReportId { get; set; }

    
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record ReportCommentDeletedEvent(Guid Id) : INotification;