using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.FireSecuritySetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.FireSecuritySetting;

[HttpPost(PreventionRoutes.FireSecuritySettings.UpdateKnownMyEnterpriseContent)]
[Tags("KnownMyEnterprise")]
public class UpdateKnownMyEnterpriseContentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateKnownMyEnterpriseContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateKnownMyEnterpriseContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}