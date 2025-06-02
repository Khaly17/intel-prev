using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Shared;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpPost(PreventionRoutes.GeoLocations.Update)]
[Tags("GeoLocations")]
public class UpdateGeoLocationEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateGeoLocationCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateGeoLocationCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}