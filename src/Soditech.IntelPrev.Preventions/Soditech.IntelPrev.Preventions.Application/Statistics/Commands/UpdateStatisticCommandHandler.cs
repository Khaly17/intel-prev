using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Statistics;

namespace Soditech.IntelPrev.Preventions.Application.Statistics.Commands;

public class UpdateStatisticCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateStatisticCommand, TResult<StatisticResult>>
{
    private readonly IRepository<Statistic> _statisticRepository = serviceProvider.GetRequiredService<IRepository<Statistic>>();
    private readonly ILogger<UpdateStatisticCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateStatisticCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<StatisticResult>> Handle(UpdateStatisticCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var statistic = await _statisticRepository.GetAsync(request.Id, cancellationToken);
            if (statistic == null)
            {
                return Result.Failure<StatisticResult>(new Error("404", "Statistic not found"));
            }
            
            _mapper.Map(request, statistic);
            
            statistic.UpdaterId = _session.UserId;
            statistic.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _statisticRepository.UpdateAsync(statistic, cancellationToken);
            await _publisher.Publish(_mapper.Map<StatisticUpdatedEvent>(statistic), cancellationToken);

            
            return Result.Success(_mapper.Map<StatisticResult>(statistic));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating statistic");

            return Result.Failure<StatisticResult>(new Error("500", "Error while updating statistic"));
        }
    }   
}