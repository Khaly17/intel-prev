using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.FireSecuritySetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.FireSecuritySetting;


[HttpPost(PreventionRoutes.FireSecuritySettings.UpdateDefinitionContent)]
[Tags("FireSecuritySettingsDefinition")]
public class UpdateFireSecuritySettingsDefinitionContentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateDefinitionContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateDefinitionContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}