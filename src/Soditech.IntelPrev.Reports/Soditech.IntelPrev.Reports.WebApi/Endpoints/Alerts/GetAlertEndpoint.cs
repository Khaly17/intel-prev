using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Alerts;

[HttpGet(ReportRoutes.Alerts.GetById)]
[Tags("Alerts")]
public class GetAlertEndpoint(IServiceProvider serviceProvider): Endpoint<GetAlertQuery, TResult<AlertResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<AlertResult>> HandleAsync(GetAlertQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}