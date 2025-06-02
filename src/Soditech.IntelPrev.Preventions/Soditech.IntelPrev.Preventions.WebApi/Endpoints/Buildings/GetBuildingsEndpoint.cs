using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Buildings;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Buildings;

[HttpGet(PreventionRoutes.Buildings.GetAll)]
[Tags("Buildings")]
public class GetBuildingsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<BuildingResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<BuildingResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetBuildingsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}