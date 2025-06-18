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
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.ReportAttachments;

[HttpGet(ReportRoutes.ReportAttachments.GetAll)]
[Tags("ReportAttachments")]
public class GetReportAttachmentsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<ReportAttachmentResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<ReportAttachmentResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetReportAttachmentsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}