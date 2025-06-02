using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.ReportAttachments;

[HttpDelete(ReportRoutes.ReportAttachments.Delete)]
[Tags("ReportAttachments")]
public class DeleteReportAttachmentEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteReportAttachmentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteReportAttachmentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}