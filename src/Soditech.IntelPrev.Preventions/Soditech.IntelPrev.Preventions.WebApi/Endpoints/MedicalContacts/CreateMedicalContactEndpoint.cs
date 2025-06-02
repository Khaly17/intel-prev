using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.MedicalContacts;

[HttpPost(PreventionRoutes.MedicalContacts.Create)]
[Tags("MedicalContacts")]
public class CreateMedicalContactEndpoint(IServiceProvider serviceProvider): Endpoint<CreateMedicalContactCommand, TResult<MedicalContactResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<MedicalContactResult>> HandleAsync(CreateMedicalContactCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}