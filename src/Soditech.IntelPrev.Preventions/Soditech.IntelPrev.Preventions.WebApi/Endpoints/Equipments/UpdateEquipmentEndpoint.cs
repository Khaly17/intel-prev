using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpPost(PreventionRoutes.Equipments.Update)]
[Tags("Equipments")]
public class UpdateEquipmentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateEquipmentCommand, TResult<EquipmentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<EquipmentResult>> HandleAsync(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}