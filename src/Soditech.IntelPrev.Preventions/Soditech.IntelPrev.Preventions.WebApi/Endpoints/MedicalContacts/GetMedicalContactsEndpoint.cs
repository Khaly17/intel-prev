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
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.MedicalContacts;

[HttpGet(PreventionRoutes.MedicalContacts.GetAll)]
[Tags("MedicalContacts")]
public class GetMedicalContactsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<MedicalContactResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<MedicalContactResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetMedicalContactsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}