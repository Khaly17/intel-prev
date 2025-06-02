using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.StaticContents;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.StaticContents;

[HttpGet(PreventionRoutes.StaticContents.GetById)]
[Tags("StaticContents")]
public class GetStaticContentEndpoint(IServiceProvider serviceProvider): Endpoint<GetStaticContentQuery, TResult<StaticContentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<StaticContentResult>> HandleAsync(GetStaticContentQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}