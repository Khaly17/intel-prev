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
using Soditech.IntelPrev.Prevensions.Shared.GatheringPoints;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.GatheringPoints.Queries;

public class GetGatheringPointsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetGatheringPointsQuery, TResult<IEnumerable<GatheringPointResult>>>
{
    private readonly IRepository<GatheringPoint> _gatheringPointRepository = serviceProvider.GetRequiredService<IRepository<GatheringPoint>>();
    private readonly ILogger<GetGatheringPointsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetGatheringPointsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<GatheringPointResult>>> Handle(GetGatheringPointsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var gatheringPoints = await _gatheringPointRepository
                .GetAll
                .Where(gatheringPoint => gatheringPoint.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var gatheringPointResults = _mapper.Map<List<GatheringPointResult>>(gatheringPoints);

            return Result.Success<IEnumerable<GatheringPointResult>>(gatheringPointResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting gatheringPoints, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<GatheringPointResult>>(new Error("500", "Error while getting gatheringPoints"));
        }
    }
}