using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.StaticContents;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.StaticContents;

[HttpGet(PreventionRoutes.StaticContents.GetAll)]
[Tags("StaticContents")]
public class GetStaticContentsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<StaticContentResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<StaticContentResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetStaticContentsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}