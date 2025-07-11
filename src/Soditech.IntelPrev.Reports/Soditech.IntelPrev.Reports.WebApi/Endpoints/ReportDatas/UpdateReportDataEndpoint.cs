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

[HttpPost(ReportRoutes.ReportDatas.Update)]
[Tags("ReportDatas")]
public class UpdateReportDataEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateReportDataCommand, TResult<ReportDataResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<ReportDataResult>> HandleAsync(UpdateReportDataCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}