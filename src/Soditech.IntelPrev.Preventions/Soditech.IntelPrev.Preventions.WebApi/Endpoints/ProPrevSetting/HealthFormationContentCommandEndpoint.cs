using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.ProPrevSetting;

[HttpPost(PreventionRoutes.ProPrevSettings.UpdateHealthFormationContent)]
[Tags("HealthFormation")]
public class HealthFormationContentCommandEndpoint(IServiceProvider serviceProvider): Endpoint<HealthFormationContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(HealthFormationContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}