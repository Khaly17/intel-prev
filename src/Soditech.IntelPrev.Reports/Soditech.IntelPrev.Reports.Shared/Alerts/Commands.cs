using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared.Alerts;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.Shared.Alerts;

public record AlertResult
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public Guid BuildingId { get; set; }
    public string BuildingName { get; set; } = string.Empty;

    public Guid? FloorId { get; set; }
    public string? FloorNumber { get; set; } = string.Empty;



}

public record CreateAlertCommand : IRequest<Result>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double Latitude { get; set; } 
    public double Longitude { get; set; } 
    public double? Altitude { get; set; }

    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }

}

public record UpdateAlertCommand : IRequest<TResult<AlertResult>>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Guid BuildingId { get; set; }
    public Guid? FloorId { get; set; }
}
public record DeleteAlertCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}
public record GetAlertQuery(Guid Id) : IRequest<TResult<AlertResult>>;
public record GetAlertsCountQuery : IRequest<TResult<int>>;
public record GetAlertsQuery : IRequest<TResult<IEnumerable<AlertResult>>>;


public record GetAlertsGroupedByTypeQuery : IRequest<TResult<IEnumerable<AlertResult>>>
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}

public record GetCountAlertsGroupedByTypeQuery : IRequest<TResult<IEnumerable<CountAlertsGroupedByTypeResult>>>
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}

public record CountAlertsGroupedByTypeResult
{
    public required string Type { get; set; }
    public int Count { get; set; }

}