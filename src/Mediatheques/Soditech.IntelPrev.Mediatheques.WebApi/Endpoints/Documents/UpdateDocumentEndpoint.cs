using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.WebApi.Endpoints.Documents;

[HttpPost(MediathequeRoutes.Documents.Update)]
[Tags("Documents")]
public class UpdateDocumentEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateDocumentCommand, TResult<DocumentResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<DocumentResult>> HandleAsync(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}