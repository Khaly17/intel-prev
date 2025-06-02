using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Statistics;

namespace Soditech.IntelPrev.Preventions.Application.Equipments.Queries;

public class GetStatisticsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetStatisticsCountQuery, TResult<int>>
{
    private readonly IRepository<Statistic> _statisticRepository = serviceProvider.GetRequiredService<IRepository<Statistic>>();
    private readonly ILogger<GetStatisticsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetStatisticsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetStatisticsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var statisticsCount = await _statisticRepository.GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(statisticsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting statistics, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting statistics"));
        }
    }
}