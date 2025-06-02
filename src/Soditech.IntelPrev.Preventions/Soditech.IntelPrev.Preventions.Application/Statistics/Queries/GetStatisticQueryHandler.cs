using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Statistics;

namespace Soditech.IntelPrev.Preventions.Application.Equipments.Queries;

public class GetStatisticQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetStatisticQuery, TResult<StatisticResult>>
{
    private readonly IRepository<Statistic> _statisticRepository = serviceProvider.GetRequiredService<IRepository<Statistic>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    
    public async Task<TResult<StatisticResult>> Handle(GetStatisticQuery request, CancellationToken cancellationToken)
    {
        var statistic = await _statisticRepository.GetAsync(request.Id, cancellationToken);

        if (statistic == null)
        {
            return Result.Failure<StatisticResult>(new Error("404", "Statistic not found"));
        }

        var statisticResult = _mapper.Map<StatisticResult>(statistic);

        return Result.Success(statisticResult);
    }
}