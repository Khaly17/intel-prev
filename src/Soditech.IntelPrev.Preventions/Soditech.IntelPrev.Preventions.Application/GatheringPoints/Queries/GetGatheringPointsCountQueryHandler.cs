using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.Application.GatheringPoints.Queries;

public class GetGatheringPointsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetGatheringPointsCountQuery, TResult<int>>
{
    private readonly IRepository<GatheringPoint> _gatheringPointRepository = serviceProvider.GetRequiredService<IRepository<GatheringPoint>>();
    private readonly ILogger<GetGatheringPointsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetGatheringPointsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetGatheringPointsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var gatheringPointsCount = await _gatheringPointRepository
                .GetAll
                .Where(gatheringPoint => gatheringPoint.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(gatheringPointsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting gatheringPoints, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting gatheringPoints"));
        }
    }
}