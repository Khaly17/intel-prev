using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Alerts;

[HttpGet(ReportRoutes.Alerts.GetAll)]
[Tags("Alerts")]
public class GetAlertsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<AlertResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<AlertResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetAlertsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}