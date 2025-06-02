using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.FireSecuritySetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.FireSecuritySetting;

[HttpGet(PreventionRoutes.FireSecuritySettings.GetKnownMyEnterpriseContent)]
[Tags("KnownMyEnterprise")]
public class GetKnownMyEnterpriseContentEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<FireSecuritySettingContentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<FireSecuritySettingContentResult>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new KnownMyEnterpriseContentQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}