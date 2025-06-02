using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.PreventionSetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.PreventionSetting;


[HttpPost(PreventionRoutes.PreventionSettings.UpdateDefinitionContent)]
[Tags("PreventionDefinition")]
public class UpdatePreventionSettingsDefinitionContentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateDefinitionContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateDefinitionContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}