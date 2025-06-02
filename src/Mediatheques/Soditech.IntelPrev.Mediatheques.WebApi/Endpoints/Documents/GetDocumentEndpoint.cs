using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.WebApi.Endpoints.Documents;

[HttpGet(MediathequeRoutes.Documents.GetById)]
[Tags("Documents")]
public class GetDocumentEndpoint(IServiceProvider serviceProvider): Endpoint<GetDocumentQuery, TResult<DocumentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<DocumentResult>> HandleAsync(GetDocumentQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}