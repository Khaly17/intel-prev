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

[HttpGet(PreventionRoutes.ProPrevSettings.GetMyLibraryContent)]
[Tags("MyLibrary")]
public class GetMyLibraryContentEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<ProPrevContentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<ProPrevContentResult>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new MyLibraryContentQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}