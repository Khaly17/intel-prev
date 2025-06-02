using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Extensions.EfCore;
using Soditech.IntelPrev.Preventions.Persistence.Extensions.Linq;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.Application.GatheringPoints.Queries;

public class GetGatheringPointsByBuildingQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetGatheringPointsByBuildingQuery, TResult<IEnumerable<GatheringPointResult>>>
{
    private readonly IRepository<GatheringPoint> _gatheringPointRepository = serviceProvider.GetRequiredService<IRepository<GatheringPoint>>();
    private readonly ILogger<GetGatheringPointsByBuildingQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetGatheringPointsByBuildingQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<GatheringPointResult>>> Handle(GetGatheringPointsByBuildingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var gatheringPoints = await _gatheringPointRepository
                .GetAll
                .Where(gatheringPoint => gatheringPoint.TenantId == _session.TenantId)
                .WhereIf(!request.BuildingId.IsNullOrEmpty(), gatheringPoint => gatheringPoint.BuildingId == request.BuildingId)
                .WhereIf(!request.FloorId.IsNullOrEmpty(), gatheringPoint => gatheringPoint.FloorId == request.FloorId)
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