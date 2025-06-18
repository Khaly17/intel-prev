using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Buildings.Queries;

public class GetBuildingQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetBuildingQuery, TResult<BuildingResult>>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<BuildingResult>> Handle(GetBuildingQuery request, CancellationToken cancellationToken)
    {
        var building = await _buildingRepository.GetAsync(request.Id, cancellationToken);

        if (building == null)
        {
            return Result.Failure<BuildingResult>(new Error("404", "Building not found"));
        }

        var buildingResult = _mapper.Map<BuildingResult>(building);

        return Result.Success(buildingResult);
    }
}