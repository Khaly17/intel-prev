using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Statistics;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Statistics;

[HttpGet(PreventionRoutes.Statistics.GetById)]
[Tags("Statistics")]
public class GetStatisticEndpoint(IServiceProvider serviceProvider): Endpoint<GetStatisticQuery, TResult<StatisticResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<StatisticResult>> HandleAsync(GetStatisticQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}