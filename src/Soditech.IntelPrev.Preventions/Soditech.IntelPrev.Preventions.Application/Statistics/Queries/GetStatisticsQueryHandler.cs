using AutoMapper;
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

public class GetStatisticsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetStatisticsQuery, TResult<IEnumerable<StatisticResult>>>
{
    private readonly IRepository<Statistic> _statisticRepository = serviceProvider.GetRequiredService<IRepository<Statistic>>();
    private readonly ILogger<GetStatisticsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetStatisticsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<StatisticResult>>> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var statistics = await _statisticRepository
                .GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var statisticResults = _mapper.Map<List<StatisticResult>>(statistics);

            return Result.Success<IEnumerable<StatisticResult>>(statisticResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting statistics, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<StatisticResult>>(new Error("500", "Error while getting statistics"));
        }
    }
}