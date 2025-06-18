using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.GatheringPoints;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.GatheringPoints.Queries;

public class GetGatheringPointQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetGatheringPointQuery, TResult<GatheringPointResult>>
{
    private readonly IRepository<GatheringPoint> _gatheringPointRepository = serviceProvider.GetRequiredService<IRepository<GatheringPoint>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<GatheringPointResult>> Handle(GetGatheringPointQuery request, CancellationToken cancellationToken)
    {
        var gatheringPoint = await _gatheringPointRepository.GetAsync(request.Id, cancellationToken);

        if (gatheringPoint == null)
        {
            return Result.Failure<GatheringPointResult>(new Error("404", "GatheringPoint not found"));
        }

        var gatheringPointResult = _mapper.Map<GatheringPointResult>(gatheringPoint);

        return Result.Success(gatheringPointResult);
    }
}