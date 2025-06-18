using System;
using MediatR;

namespace Soditech.IntelPrev.Reports.Shared.ReportDatas;

public record ReportDataCreatedEvent : INotification
{
    public string Value { get; set; } = string.Empty;
    
    public Guid ReportId { get; set; }

    
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record ReportDataUpdatedEvent : INotification
{
    public string Value { get; set; } = string.Empty;
    
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

public record ReportDataDeletedEvent(Guid Id) : INotification;