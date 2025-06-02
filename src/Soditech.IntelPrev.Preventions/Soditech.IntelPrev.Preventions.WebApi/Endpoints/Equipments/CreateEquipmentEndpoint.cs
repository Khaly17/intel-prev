using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpPost(PreventionRoutes.Equipments.Create)]
[Tags("Equipments")]
public class CreateEquipmentEndpoint(IServiceProvider serviceProvider): Endpoint<CreateEquipmentCommand, TResult<EquipmentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<EquipmentResult>> HandleAsync(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}