using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.Application.Documents.Queries;

public class GetDocumentsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetDocumentsQuery, TResult<IEnumerable<DocumentResult>>>
{
    private readonly IRepository<Document> _documentRepository = serviceProvider.GetRequiredService<IRepository<Document>>();
    private readonly ILogger<GetDocumentsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetDocumentsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<DocumentResult>>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var documents = await _documentRepository.GetAll
                .Where(building => building.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var documentResults = _mapper.Map<IEnumerable<DocumentResult>>(documents);


            return Result.Success(documentResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting documents, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<DocumentResult>>(new Error("500", "Error while getting documents"));
        }
    }
}

// extension method `ForEach`
public static class LinqExtensions
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }

        return source;
    }
}