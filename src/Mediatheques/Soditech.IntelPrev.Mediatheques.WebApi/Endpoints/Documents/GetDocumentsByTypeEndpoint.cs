using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.WebApi.Endpoints.Documents;

[HttpGet(MediathequeRoutes.Documents.GetAllByType)]
[Tags("Documents")]
public class GetDocumentsByTypeEndpoint(IServiceProvider serviceProvider): Endpoint<GetDocumentsByTypeQuery, TResult<IEnumerable<DocumentResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<DocumentResult>>> HandleAsync(GetDocumentsByTypeQuery request, CancellationToken cancellationToken=default)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}