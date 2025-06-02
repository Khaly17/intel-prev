using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Campaigns;

[HttpGet(PreventionRoutes.Campaigns.GetAll)]
[Tags("Campaigns")]
public class GetCampaignsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<CampaignResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<CampaignResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetCampaignsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}