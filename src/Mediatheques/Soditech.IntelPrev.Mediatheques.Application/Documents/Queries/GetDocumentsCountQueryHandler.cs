using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.Application.Documents.Queries;

public class GetDocumentsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetDocumentsCountQuery, TResult<int>>
{
    private readonly IRepository<Document> _documentRepository = serviceProvider.GetRequiredService<IRepository<Document>>();
    private readonly ILogger<GetDocumentsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetDocumentsCountQueryHandler>>();

    public async Task<TResult<int>> Handle(GetDocumentsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var documentsCount = await _documentRepository.CountAsync(cancellationToken: cancellationToken);
                
            return Result.Success(documentsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting documents, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting documents"));
        }
    }
}