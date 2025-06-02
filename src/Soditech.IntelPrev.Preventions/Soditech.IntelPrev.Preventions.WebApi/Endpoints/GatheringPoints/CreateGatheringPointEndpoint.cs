using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.GatheringPoints;

[HttpPost(PreventionRoutes.GatheringPoints.Create)]
[Tags("GatheringPoints")]
public class CreateGatheringPointEndpoint(IServiceProvider serviceProvider): Endpoint<CreateGatheringPointCommand, TResult<GatheringPointResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<GatheringPointResult>> HandleAsync(CreateGatheringPointCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}