using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.GeoLocations;

[HttpPost(PreventionRoutes.GeoLocations.Update)]
[Tags("GeoLocations")]
public class UpdateGeoLocationEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateGeoLocationCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(UpdateGeoLocationCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}