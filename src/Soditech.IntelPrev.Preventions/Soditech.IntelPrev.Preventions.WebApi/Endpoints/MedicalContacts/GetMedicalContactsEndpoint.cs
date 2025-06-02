using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.MedicalContacts;

[HttpGet(PreventionRoutes.MedicalContacts.GetAll)]
[Tags("MedicalContacts")]
public class GetMedicalContactsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<MedicalContactResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<MedicalContactResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetMedicalContactsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}