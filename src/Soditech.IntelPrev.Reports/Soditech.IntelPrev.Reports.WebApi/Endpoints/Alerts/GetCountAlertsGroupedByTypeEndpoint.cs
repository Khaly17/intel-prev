using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Alerts;

[HttpGet(ReportRoutes.Alerts.GetCountAlertsGroupedByType)]
[Tags("Alerts")]
public class GetCountAlertsGroupedByTypeEndpoint(IServiceProvider serviceProvider): Endpoint<GetCountAlertsGroupedByTypeQuery, TResult<IEnumerable<CountAlertsGroupedByTypeResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<CountAlertsGroupedByTypeResult>>> HandleAsync(GetCountAlertsGroupedByTypeQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}