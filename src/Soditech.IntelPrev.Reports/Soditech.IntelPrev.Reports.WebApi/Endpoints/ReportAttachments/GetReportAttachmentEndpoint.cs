using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.ReportAttachments;

[HttpGet(ReportRoutes.ReportAttachments.GetById)]
[Tags("ReportAttachments")]
public class GetReportAttachmentEndpoint(IServiceProvider serviceProvider): Endpoint<GetReportAttachmentQuery, TResult<ReportAttachmentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<ReportAttachmentResult>> HandleAsync(GetReportAttachmentQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}