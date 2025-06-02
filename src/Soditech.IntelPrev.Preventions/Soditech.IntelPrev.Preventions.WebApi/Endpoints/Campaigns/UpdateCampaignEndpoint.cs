using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Campaigns;

[HttpPost(PreventionRoutes.Campaigns.Update)]
[Tags("Campaigns")]
public class UpdateCampaignEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateCampaignCommand, TResult<CampaignResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<CampaignResult>> HandleAsync(UpdateCampaignCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}