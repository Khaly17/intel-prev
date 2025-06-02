using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Shared;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpPost(PreventionRoutes.GeoLocations.Create)]
[Tags("GeoLocations")]
public class CreateGeoLocationEndpoint(IServiceProvider serviceProvider): Endpoint<AddGeoLocationCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(AddGeoLocationCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}