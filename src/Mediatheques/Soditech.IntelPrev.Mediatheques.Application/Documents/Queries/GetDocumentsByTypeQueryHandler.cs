using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;
using Soditech.IntelPrev.Mediatheques.Shared.Enums;

namespace Soditech.IntelPrev.Documents.Application.Documents.Queries;

public class GetDocumentsByTypeQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetDocumentsByTypeQuery, TResult<IEnumerable<DocumentResult>>>
{
    private readonly IRepository<Document> _documentRepository = serviceProvider.GetRequiredService<IRepository<Document>>();
    private readonly ILogger<GetDocumentsByTypeQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetDocumentsByTypeQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    

    public async Task<TResult<IEnumerable<DocumentResult>>> Handle(GetDocumentsByTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (Enum.TryParse<DocumentType>(request.Type, out var type))
            {
                var documents = await _documentRepository.GetAll
                    .Where(building => building.TenantId == _session.TenantId)
                    .Where(building => building.Type == type)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var documentResults = _mapper.Map<IEnumerable<DocumentResult>>(documents);

                return Result.Success(documentResults);
            }
            else
            {
                return Result.Failure<IEnumerable<DocumentResult>>(new Error("400", "Invalid document type"));
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting documents, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<DocumentResult>>(new Error("500", "Error while getting documents"));
        }
    }
}