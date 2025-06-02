using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Reports;

[HttpGet(ReportRoutes.Reports.GetReportsGroupedByRegister)]
[Tags("Reports")]
public class GetReportsGroupedByRegisterEndpoint(IServiceProvider serviceProvider): Endpoint<GetReportsGroupedByRegisterQuery, TResult<IEnumerable<ReportResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<ReportResult>>> HandleAsync(GetReportsGroupedByRegisterQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}