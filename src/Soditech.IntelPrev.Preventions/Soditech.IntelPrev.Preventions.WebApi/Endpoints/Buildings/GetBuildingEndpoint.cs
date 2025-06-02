using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Buildings;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Buildings;

[HttpGet(PreventionRoutes.Buildings.GetById)]
[Tags("Buildings")]
public class GetBuildingEndpoint(IServiceProvider serviceProvider): Endpoint<GetBuildingQuery, TResult<BuildingResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<BuildingResult>> HandleAsync(GetBuildingQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}