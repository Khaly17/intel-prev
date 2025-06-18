using System;
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

[HttpGet(ReportRoutes.ReportDatas.GetById)]
[Tags("ReportDatas")]
public class GetReportDataEndpoint(IServiceProvider serviceProvider): Endpoint<GetReportDataQuery, TResult<ReportDataResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<ReportDataResult>> HandleAsync(GetReportDataQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}