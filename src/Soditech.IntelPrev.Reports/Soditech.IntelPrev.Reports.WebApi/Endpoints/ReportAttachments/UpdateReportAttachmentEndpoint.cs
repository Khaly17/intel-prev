using System;
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

[HttpPost(ReportRoutes.ReportAttachments.Update)]
[Tags("ReportAttachments")]
public class UpdateReportAttachmentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateReportAttachmentCommand, TResult<ReportAttachmentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<ReportAttachmentResult>> HandleAsync(UpdateReportAttachmentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}