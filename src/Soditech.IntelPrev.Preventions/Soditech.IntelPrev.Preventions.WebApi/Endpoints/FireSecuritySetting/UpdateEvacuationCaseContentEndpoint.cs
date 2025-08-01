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

[HttpPost(PreventionRoutes.FireSecuritySettings.UpdateEvacuationCaseContent)]
[Tags("EvacuationCaseContent")]
public class UpdateEvacuationCaseContentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateEvacuationCaseContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateEvacuationCaseContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}