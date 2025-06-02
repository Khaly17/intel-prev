using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.FireSecuritySetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.FireSecuritySetting;

[HttpPost(PreventionRoutes.FireSecuritySettings.UpdateFireMaterialsContent)]
[Tags("FireMaterialsContent")]
public class UpdateFireMaterialsContentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateFireMaterialsContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateFireMaterialsContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}