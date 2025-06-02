using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.ReportComments;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.ReportComments;

[HttpPost(ReportRoutes.ReportComments.Create)]
[Tags("ReportComments")]
public class CreateReportCommentEndpoint(IServiceProvider serviceProvider): Endpoint<CreateReportCommentCommand, TResult<ReportCommentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<ReportCommentResult>> HandleAsync(CreateReportCommentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}