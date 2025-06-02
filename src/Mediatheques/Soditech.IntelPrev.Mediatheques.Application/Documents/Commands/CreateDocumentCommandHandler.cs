using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.Application.Documents.Commands;


public class CreateDocumentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateDocumentCommand, TResult<DocumentResult>>
{
    private readonly IRepository<Document> _documentRepository = serviceProvider.GetRequiredService<IRepository<Document>>();
    private readonly ILogger<CreateDocumentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateDocumentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ISender _sender = serviceProvider.GetRequiredService<ISender>();

    public async Task<TResult<DocumentResult>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<DocumentResult>(new Error("400", "cannot create document without a tenant"));
            }

            if (request.BlobFile is not { Length: > 0 })
            {
                return Result.Failure<DocumentResult>(new Error("400", "cannot create document without a file"));
            }

            var document = _mapper.Map<Document>(request);
            document.TenantId = _session.TenantId.Value;
            
            document.CreatorId = _session.UserId;
            document.CreatedAt = DateTimeOffset.UtcNow;

            document.Id = Guid.NewGuid();
            document.Path = $"{document.Id}{request.Extension}";

            await _documentRepository.AddAsync(document, cancellationToken);
           
            var createFileFormByteCommandResult= await _sender.Send(new CreateFileFormByteCommand
            {
                FileName = document.Path,
                BlobFile = request.BlobFile
            }, cancellationToken);
            
            if (createFileFormByteCommandResult.IsFailure)
            {
                //TODO: delete the document
                return Result.Failure<DocumentResult>(createFileFormByteCommandResult.Error);
            }
            
            //TODO: notify users that a new document has been created.
            
            return Result.Success(_mapper.Map<DocumentResult>(document));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create document");
        }
        
        return Result.Failure<DocumentResult>(new Error("500", "Error while creating document"));
    }   
}