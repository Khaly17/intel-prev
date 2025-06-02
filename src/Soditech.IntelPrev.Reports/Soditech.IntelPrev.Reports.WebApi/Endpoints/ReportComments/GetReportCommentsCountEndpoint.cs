using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.ReportComments;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.ReportComments;

[HttpGet(ReportRoutes.ReportComments.Count)]
[Tags("ReportComments")]
public class GetReportCommentsCountEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<int>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<int>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetReportCommentsCountQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}