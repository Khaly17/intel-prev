using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.Application.Documents.Commands;

public class DeleteDocumentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteDocumentCommand, Result>
{
    private readonly IRepository<Document> _documentRepository = serviceProvider.GetRequiredService<IRepository<Document>>();
    private readonly ILogger<DeleteDocumentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteDocumentCommandHandler>>();

    public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _documentRepository.GetAsync(request.Id, cancellationToken);
            if (document == null)
            {
                return Result.Failure<DocumentResult>(new Error("404", "Document not found"));
            }
            
            await _documentRepository.DeleteAsync(document, cancellationToken);
            
            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting document");

            return Result.Failure(new Error("500", "Error while deleting document"));
        }
    }   
}