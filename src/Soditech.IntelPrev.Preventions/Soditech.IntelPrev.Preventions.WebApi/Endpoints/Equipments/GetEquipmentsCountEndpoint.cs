using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Equipments;

[HttpGet(PreventionRoutes.Equipments.Count)]
[Tags("Equipments")]
public class GetEquipmentsCountEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<int>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<int>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetEquipmentsCountQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}