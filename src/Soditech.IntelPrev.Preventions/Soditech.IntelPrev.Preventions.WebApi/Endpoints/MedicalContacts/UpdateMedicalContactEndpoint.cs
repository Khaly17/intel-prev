using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.MedicalContacts;

[HttpPost(PreventionRoutes.MedicalContacts.Update)]
[Tags("MedicalContacts")]
public class UpdateMedicalContactEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateMedicalContactCommand, TResult<MedicalContactResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<MedicalContactResult>> HandleAsync(UpdateMedicalContactCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}