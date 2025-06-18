using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Statistics;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Statistics.Commands;

public class DeleteStatisticCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteStatisticCommand, Result>
{
    private readonly IRepository<Statistic> _statisticRepository = serviceProvider.GetRequiredService<IRepository<Statistic>>();
    private readonly ILogger<DeleteStatisticCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteStatisticCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteStatisticCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var statistic = await _statisticRepository.GetAsync(request.Id, cancellationToken);
            if (statistic == null)
            {
                return Result.Failure<StatisticResult>(new Error("404", "Statistic not found"));
            }
            
            await _statisticRepository.DeleteAsync(statistic, cancellationToken);
            
            await _publisher.Publish(new StatisticDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting statistic");

            return Result.Failure(new Error("500", "Error while deleting statistic"));
        }
    }   
}