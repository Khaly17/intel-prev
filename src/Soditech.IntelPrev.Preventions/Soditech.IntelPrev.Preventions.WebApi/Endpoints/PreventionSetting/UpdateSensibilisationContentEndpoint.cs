using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.PreventionSetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.PreventionSetting;

[HttpPost(PreventionRoutes.PreventionSettings.UpdateSensibilisationContent)]
[Tags("SensibilisationContent")]
public class UpdateSensibilisationContentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateSensibilisationContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateSensibilisationContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}