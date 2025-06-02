using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Events;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Events;

[HttpGet(PreventionRoutes.Events.GetUpComing)]
[Tags("Events")]
public class GetUpComingEventsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<EventResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetUpComingEventsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}