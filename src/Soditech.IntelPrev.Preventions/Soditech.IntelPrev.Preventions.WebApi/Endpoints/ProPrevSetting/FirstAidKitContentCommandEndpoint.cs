using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.ProPrevSetting;

[HttpPost(PreventionRoutes.ProPrevSettings.UpdateFirstAidKitContent)]
[Tags("FirstAidKit")]
public class FirstAidKitContentCommandEndpoint(IServiceProvider serviceProvider): Endpoint<FirstAidKitContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(FirstAidKitContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}