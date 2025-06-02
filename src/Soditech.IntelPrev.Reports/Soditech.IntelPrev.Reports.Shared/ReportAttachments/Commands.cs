using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Reports.Shared.ReportAttachments;

public record ReportAttachmentResult
{
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; //(e.g., "image/png", "application/pdf").
    public string FilePath { get; set; } = string.Empty;
    
    public Guid ReportId { get; set; }
    public string ReportTitle { get; set; } = default!;
    
    
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
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

public record CreateReportAttachmentCommand : IRequest<TResult<ReportAttachmentResult>>
{
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public Guid ReportId { get; set; }
}

public record UpdateReportAttachmentCommand : IRequest<TResult<ReportAttachmentResult>>
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public Guid ReportId { get; set; }
}

public record DeleteReportAttachmentCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetReportAttachmentQuery : IRequest<TResult<ReportAttachmentResult>>
{
    public Guid Id { get; set; }
}

public record GetReportAttachmentsCountQuery : IRequest<TResult<int>>;

public record GetReportAttachmentsQuery : IRequest<TResult<IEnumerable<ReportAttachmentResult>>>
{
    public Guid ReportId { get; set; }
}

public record GetReportAttachmentByReportIdQuery : IRequest<TResult<ReportAttachmentResult>>
{
    public Guid ReportId { get; set; }
}

public record GetReportAttachmentByReportIdAndFileNameQuery : IRequest<TResult<ReportAttachmentResult>>
{
    public Guid ReportId { get; set; }
    public string FileName { get; set; } = string.Empty;
}

