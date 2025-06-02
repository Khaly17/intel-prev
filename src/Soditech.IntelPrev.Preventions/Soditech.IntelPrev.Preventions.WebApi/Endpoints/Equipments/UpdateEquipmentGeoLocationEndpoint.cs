using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpPost(PreventionRoutes.Equipments.UpdateGeoLocation)]
[Tags("Equipments")]
public class UpdateEquipmentGeoLocationEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateEquipmentGeoLocationCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateEquipmentGeoLocationCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}