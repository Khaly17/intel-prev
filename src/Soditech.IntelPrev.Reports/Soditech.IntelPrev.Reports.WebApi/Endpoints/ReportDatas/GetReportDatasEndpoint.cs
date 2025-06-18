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
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.ReportDatas;

[HttpGet(ReportRoutes.ReportDatas.GetAll)]
[Tags("ReportDatas")]
public class GetReportDatasEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<ReportDataResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<ReportDataResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetReportDatasQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}