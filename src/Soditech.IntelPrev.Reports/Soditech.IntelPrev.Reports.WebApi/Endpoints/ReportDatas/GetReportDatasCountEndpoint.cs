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

[HttpGet(ReportRoutes.ReportDatas.Count)]
[Tags("ReportDatas")]
public class GetReportDatasCountEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<int>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<int>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetReportDatasCountQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}