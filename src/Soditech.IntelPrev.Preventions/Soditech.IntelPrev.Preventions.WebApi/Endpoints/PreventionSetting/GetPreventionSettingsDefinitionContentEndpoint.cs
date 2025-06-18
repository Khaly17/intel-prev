using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.PreventionSetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.PreventionSetting;

[HttpGet(PreventionRoutes.PreventionSettings.GetDefinitionContent)]
[Tags("PreventionDefinition")]
public class GetPreventionSettingsDefinitionContentEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<PreventionContentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<PreventionContentResult>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new DefinitionContentQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}