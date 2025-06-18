using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.RegistreFields;

[HttpGet(ReportRoutes.RegisterFields.GetAll)]
[Tags("RegisterFields")]
public class GetRegisterFieldsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<RegisterFieldResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<RegisterFieldResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetRegisterFieldsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}