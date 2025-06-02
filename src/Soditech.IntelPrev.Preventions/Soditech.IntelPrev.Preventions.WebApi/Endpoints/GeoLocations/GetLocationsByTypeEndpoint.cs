using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Shared;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpGet(PreventionRoutes.GeoLocations.GetAllByType)]
[Tags("GeoLocations")]
public class GetLocationsByTypeEndpoint(IServiceProvider serviceProvider): Endpoint<GetLocationsByTypeQuery, TResult<IEnumerable<GeoLocationResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<GeoLocationResult>>> HandleAsync(GetLocationsByTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}