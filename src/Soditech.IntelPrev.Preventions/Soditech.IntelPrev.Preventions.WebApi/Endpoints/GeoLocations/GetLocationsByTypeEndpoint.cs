using System;
using System.Collections.Generic;
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

[HttpGet(PreventionRoutes.GeoLocations.GetAllByType)]
[Tags("GeoLocations")]
public class GetLocationsByTypeEndpoint(IServiceProvider serviceProvider): Endpoint<GetLocationsByTypeQuery, TResult<IEnumerable<GeoLocationResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<GeoLocationResult>>> HandleAsync(GetLocationsByTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}