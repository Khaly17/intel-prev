using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Reports.Shared.ReportComments;

public record ReportCommentResult
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
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

public record CreateReportCommentCommand : IRequest<TResult<ReportCommentResult>>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid ReportId { get; set; }
}

public record UpdateReportCommentCommand : IRequest<TResult<ReportCommentResult>>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid ReportId { get; set; }
}

public record DeleteReportCommentCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetReportCommentQuery : IRequest<TResult<ReportCommentResult>>
{
    public Guid Id { get; set; }
}

public record GetReportCommentsCountQuery : IRequest<TResult<int>>;

public record GetReportCommentsQuery : IRequest<TResult<IEnumerable<ReportCommentResult>>>
{
    public Guid ReportId { get; set; }
}