using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.StaticContents;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.StaticContents;

[HttpPost(PreventionRoutes.StaticContents.Update)]
[Tags("StaticContents")]
public class UpdateStaticContentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateStaticContentCommand, TResult<StaticContentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<StaticContentResult>> HandleAsync(UpdateStaticContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}