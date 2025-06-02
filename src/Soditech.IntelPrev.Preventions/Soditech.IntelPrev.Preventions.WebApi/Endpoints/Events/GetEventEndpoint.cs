using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Events;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Events;

[HttpGet(PreventionRoutes.Events.GetById)]
[Tags("Events")]
public class GetEventEndpoint(IServiceProvider serviceProvider): Endpoint<GetEventQuery, TResult<EventResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<EventResult>> HandleAsync(GetEventQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}