using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Buildings;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Buildings;

[HttpPost(PreventionRoutes.Buildings.Update)]
[Tags("Buildings")]
public class UpdateBuildingEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateBuildingCommand, TResult<BuildingResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<BuildingResult>> HandleAsync(UpdateBuildingCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}