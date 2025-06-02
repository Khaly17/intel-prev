using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Reports;

[HttpGet(ReportRoutes.Reports.GetCountReportsGroupedByRegister)]
[Tags("Reports")]
public class GetCountReportsGroupedByRegisterEndpoint(IServiceProvider serviceProvider): Endpoint<GetCountReportsGroupedByRegisterQuery, TResult<IEnumerable<CountReportsGroupedByRegisterResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<CountReportsGroupedByRegisterResult>>> HandleAsync(GetCountReportsGroupedByRegisterQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}