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
using Soditech.IntelPrev.Prevensions.Shared.StaticContents;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.StaticContents.Queries;

public class GetStaticContentsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetStaticContentsQuery, TResult<IEnumerable<StaticContentResult>>>
{
    private readonly IRepository<StaticContent> _staticContentRepository = serviceProvider.GetRequiredService<IRepository<StaticContent>>();
    private readonly ILogger<GetStaticContentsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetStaticContentsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<StaticContentResult>>> Handle(GetStaticContentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var staticContents = await _staticContentRepository
                .GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var staticContentResults = _mapper.Map<List<StaticContentResult>>(staticContents);

            return Result.Success<IEnumerable<StaticContentResult>>(staticContentResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting staticContents, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<StaticContentResult>>(new Error("500", "Error while getting staticContents"));
        }
    }
}