using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.WebApi.Endpoints.Documents;

[HttpGet(MediathequeRoutes.Documents.GetAll)]
[Tags("Documents")]
public class GetDocumentsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<DocumentResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<DocumentResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetDocumentsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}