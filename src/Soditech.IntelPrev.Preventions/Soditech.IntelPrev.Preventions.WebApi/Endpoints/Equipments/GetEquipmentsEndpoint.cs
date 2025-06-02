using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpGet(PreventionRoutes.Equipments.GetAll)]
[Tags("Equipments")]
public class GetEquipmentsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<EquipmentResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<EquipmentResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetEquipmentsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}