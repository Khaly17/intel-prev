using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.PreventionSetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.PreventionSetting;

[HttpGet(PreventionRoutes.PreventionSettings.GetSensibilisationContent)]
[Tags("SensibilisationContent")]
public class GetSensibilisationContentContentEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<PreventionContentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<PreventionContentResult>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new SensibilisationContentQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}