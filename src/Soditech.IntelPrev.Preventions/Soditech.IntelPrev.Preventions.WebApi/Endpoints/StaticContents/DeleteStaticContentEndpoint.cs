using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.StaticContents;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.StaticContents;

[HttpDelete(PreventionRoutes.StaticContents.Delete)]
[Tags("StaticContents")]
public class DeleteStaticContentEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteStaticContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteStaticContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}