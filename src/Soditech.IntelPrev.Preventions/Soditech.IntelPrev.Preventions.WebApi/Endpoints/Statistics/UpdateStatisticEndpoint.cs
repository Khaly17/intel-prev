using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Statistics;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Statistics;

[HttpPost(PreventionRoutes.Statistics.Update)]
[Tags("Statistics")]
public class UpdateStatisticEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateStatisticCommand, TResult<StatisticResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<StatisticResult>> HandleAsync(UpdateStatisticCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}