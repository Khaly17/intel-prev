using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpDelete(PreventionRoutes.Equipments.Delete)]
[Tags("Equipments")]
public class DeleteEquipmentEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteEquipmentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}