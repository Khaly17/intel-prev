using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.MedicalContacts;

[HttpDelete(PreventionRoutes.MedicalContacts.Delete)]
[Tags("MedicalContacts")]
public class DeleteMedicalContactEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteMedicalContactCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteMedicalContactCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}