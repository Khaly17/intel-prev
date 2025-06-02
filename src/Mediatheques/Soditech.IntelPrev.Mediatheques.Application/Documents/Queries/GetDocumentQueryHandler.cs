using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Application.Documents.Commands;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Documents.Application.Documents.Queries;

public class GetDocumentQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetDocumentQuery, TResult<DocumentResult>>
{
    private readonly IRepository<Document> _documentRepository = serviceProvider.GetRequiredService<IRepository<Document>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    
    public async Task<TResult<DocumentResult>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetAsync(request.Id, cancellationToken);

        if (document == null)
        {
            return Result.Failure<DocumentResult>(new Error("404", "Document not found"));
        }

        var documentResult = _mapper.Map<DocumentResult>(document);

        return Result.Success(documentResult);
    }
}