using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Statistics;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Statistics;

[HttpGet(PreventionRoutes.Statistics.GetAll)]
[Tags("Statistics")]
public class GetStatisticsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<StatisticResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<StatisticResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetStatisticsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}