using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Reports.Shared.ReportDatas;

public record ReportDataResult
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Value { get; set; } = string.Empty; // Value of the property
    
    public Guid ReportId { get; set; } // reference to the register
    public string ReportName { get; set; } = default!;
    
    public Guid FieldId { get; set; } // reference to the field
    public string FieldName { get; set; } = default!;
    
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

public record CreateReportDataCommand : IRequest<TResult<ReportDataResult>>
{
    public Guid ReportId { get; set; }
    public Guid FieldId { get; set; }
    public string Value { get; set; } = string.Empty;
}

public record UpdateReportDataCommand : IRequest<TResult<ReportDataResult>>
{
    public Guid Id { get; set; }
    public string Value { get; set; } = string.Empty;
}

public record DeleteReportDataCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetReportDataQuery : IRequest<TResult<ReportDataResult>>
{
    public Guid Id { get; set; }
}

public record GetReportDatasCountQuery : IRequest<TResult<int>>;

public record GetReportDatasQuery : IRequest<TResult<IEnumerable<ReportDataResult>>>;

public record GetReportDataByReportIdQuery : IRequest<TResult<IEnumerable<ReportDataResult>>>
{
    public Guid ReportId { get; set; }
}


