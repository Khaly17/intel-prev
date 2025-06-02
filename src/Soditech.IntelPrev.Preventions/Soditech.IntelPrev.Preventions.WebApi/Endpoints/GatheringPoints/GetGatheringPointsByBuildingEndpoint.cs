using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.GatheringPoints;

[HttpGet(PreventionRoutes.GatheringPoints.GetGatheringPointsByBuilding)]
[Tags("GatheringPoints")]
public class GetGatheringPointsByBuildingEndpoint(IServiceProvider serviceProvider): Endpoint<GetGatheringPointsByBuildingQuery, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(GetGatheringPointsByBuildingQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}