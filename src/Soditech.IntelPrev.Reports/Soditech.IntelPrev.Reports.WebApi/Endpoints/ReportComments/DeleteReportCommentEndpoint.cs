using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.ReportComments;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.ReportComments;

[HttpDelete(ReportRoutes.ReportComments.Delete)]
[Tags("ReportComments")]
public class DeleteReportCommentEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteReportCommentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteReportCommentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}