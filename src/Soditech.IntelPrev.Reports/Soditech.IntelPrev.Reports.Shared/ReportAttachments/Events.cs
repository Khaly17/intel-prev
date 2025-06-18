using System;
using MediatR;

namespace Soditech.IntelPrev.Reports.Shared.ReportAttachments;

public record ReportAttachmentCreatedEvent : INotification
{
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; //(e.g., "image/png", "application/pdf").
    public string FilePath { get; set; } = string.Empty;
    
    public Guid ReportId { get; set; }

    
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record ReportAttachmentUpdatedEvent : INotification
{
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; //(e.g., "image/png", "application/pdf").
    public string FilePath { get; set; } = string.Empty;
    
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

public record ReportAttachmentDeletedEvent(Guid Id) : INotification;