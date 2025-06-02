using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Buildings;

namespace Soditech.IntelPrev.Preventions.Application.Buildings.Queries;

public class GetBuildingsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetBuildingsCountQuery, TResult<int>>
{
    private readonly IRepository<Building> _buildingRepository = serviceProvider.GetRequiredService<IRepository<Building>>();
    private readonly ILogger<GetBuildingsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetBuildingsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetBuildingsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var buildingsCount = await _buildingRepository
                .GetAll
                .Where(building => building.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(buildingsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting buildings, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting buildings"));
        }
    }
}