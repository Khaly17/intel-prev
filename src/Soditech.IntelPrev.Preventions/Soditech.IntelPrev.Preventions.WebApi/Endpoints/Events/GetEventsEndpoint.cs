using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Events;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Events;

[HttpGet(PreventionRoutes.Events.GetAll)]
[Tags("Events")]
public class GetEventsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<EventResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<EventResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetEventsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}