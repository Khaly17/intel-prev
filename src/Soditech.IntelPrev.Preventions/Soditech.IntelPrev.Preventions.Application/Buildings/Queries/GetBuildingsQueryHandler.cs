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
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Buildings.Queries;

public class GetBuildingsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetBuildingsQuery, TResult<IEnumerable<BuildingResult>>>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly ILogger<GetBuildingsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetBuildingsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<BuildingResult>>> Handle(GetBuildingsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var buildings = await _buildingRepository
                .GetAll
                .Where(building => building.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var buildingResults = _mapper.Map<IEnumerable<BuildingResult>>(buildings);

            return Result.Success<IEnumerable<BuildingResult>>(buildingResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting buildings, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<BuildingResult>>(new Error("500", "Error while getting buildings"));
        }
    }
}