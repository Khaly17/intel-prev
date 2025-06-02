using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.GatheringPoints;

[HttpGet(PreventionRoutes.GatheringPoints.GetAll)]
[Tags("GatheringPoints")]
public class GetGatheringPointsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<GatheringPointResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<GatheringPointResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetGatheringPointsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}