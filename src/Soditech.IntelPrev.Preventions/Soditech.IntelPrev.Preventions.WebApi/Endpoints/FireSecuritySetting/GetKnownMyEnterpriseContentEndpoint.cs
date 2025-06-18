using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.FireSecuritySetting;

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