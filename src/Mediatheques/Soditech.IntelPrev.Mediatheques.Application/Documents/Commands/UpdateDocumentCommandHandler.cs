using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Users.Application.Documents.Commands;

public class UpdateDocumentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateDocumentCommand, TResult<DocumentResult>>
{
    private readonly IRepository<Document> _documentRepository = serviceProvider.GetRequiredService<IRepository<Document>>();
    private readonly ILogger<UpdateDocumentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateDocumentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
  
    public async Task<TResult<DocumentResult>> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _documentRepository.GetAsync(request.Id, cancellationToken);
            if (document == null)
            {
                return Result.Failure<DocumentResult>(new Error("404", "Document not found"));
            }
            
            _mapper.Map(request, document);
            
            //TODO: Normalize the name of the document
            
            await _documentRepository.UpdateAsync(document, cancellationToken);
            
            return Result.Success(_mapper.Map<DocumentResult>(document));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating document");

            return Result.Failure<DocumentResult>(new Error("500", "Error while updating document"));
        }
    }   
}