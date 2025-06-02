using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Alerts;

[HttpGet(ReportRoutes.Alerts.GetAlertsByPeriod)]
[Tags("Alerts")]
public class GetAlertsByPeriodEndpoint(IServiceProvider serviceProvider): Endpoint<GetAlertsGroupedByTypeQuery, TResult<IEnumerable<AlertResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<AlertResult>>> HandleAsync(GetAlertsGroupedByTypeQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}