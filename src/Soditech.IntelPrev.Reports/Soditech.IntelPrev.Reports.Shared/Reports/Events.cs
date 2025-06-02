using MediatR;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.Shared.Reports;


public record ReportCreatedEvent : INotification
{
    public string Title { get; set; } = string.Empty; // Register title
    public string Description { get; set; }  = string.Empty; // General description 
    public string Status { get; set; } = string.Empty;

    
    public Guid RegisterTypeId { get; set; } // reference to the register
    public List<ReportDataResult> ReportDatas { get; set; } = default!;
    
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}

public record ReportUpdatedEvent : INotification
{
    public string Title { get; set; } = string.Empty; // Register title
    public string Description { get; set; }  = string.Empty; // General description 
    public string Status { get; set; } = string.Empty;

    
    public Guid RegisterTypeId { get; set; } // reference to the register
    public List<ReportDataResult> ReportDatas { get; set; } = default!;
    
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record ReportDeletedEvent(Guid Id) : INotification;