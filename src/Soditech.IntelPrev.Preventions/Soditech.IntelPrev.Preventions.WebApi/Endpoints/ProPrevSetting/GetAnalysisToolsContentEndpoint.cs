using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.ProPrevSetting;

[HttpGet(PreventionRoutes.ProPrevSettings.GetAnalysisToolsContent)]
[Tags("AnalysisTools")]
public class GetAnalysisToolsContentEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<ProPrevContentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<ProPrevContentResult>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new AnalysisToolsContentQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}