using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Campaigns;

[HttpDelete(PreventionRoutes.Campaigns.Delete)]
[Tags("Campaigns")]
public class DeleteCampaignEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteCampaignCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteCampaignCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}