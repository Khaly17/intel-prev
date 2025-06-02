using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.ProPrevSetting;

[HttpPost(PreventionRoutes.ProPrevSettings.UpdateMyLibraryContent)]
[Tags("MyLibrary")]
public class MyLibraryContentCommandEndpoint(IServiceProvider serviceProvider): Endpoint<MyLibraryContentCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(MyLibraryContentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}