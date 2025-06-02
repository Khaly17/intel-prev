using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.GatheringPoints;

[HttpDelete(PreventionRoutes.GatheringPoints.Delete)]
[Tags("GatheringPoints")]
public class DeleteGatheringPointEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteGatheringPointCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteGatheringPointCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}