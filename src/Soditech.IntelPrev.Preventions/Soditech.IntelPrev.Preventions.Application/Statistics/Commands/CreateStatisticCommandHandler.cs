using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.Statistics;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Statistics.Commands;

public class CreateStatisticCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateStatisticCommand, TResult<StatisticResult>>
{
    private readonly IRepository<Statistic> _statisticRepository = serviceProvider.GetRequiredService<IRepository<Statistic>>();
    private readonly ILogger<CreateStatisticCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateStatisticCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<StatisticResult>> Handle(CreateStatisticCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<StatisticResult>(new Error("400", "cannot create statistic without a tenant"));
            }
            var statistic = _mapper.Map<Statistic>(request);
            statistic.TenantId = _session.TenantId.Value;
            
            statistic.CreatorId = _session.UserId;
            statistic.CreatedAt = DateTimeOffset.UtcNow;

            await _statisticRepository.AddAsync(statistic, cancellationToken);

            await _publisher.Publish(_mapper.Map<StatisticCreatedEvent>(statistic), cancellationToken);
            
            return Result.Success(_mapper.Map<StatisticResult>(statistic));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create statistic");
        }
        
        return Result.Failure<StatisticResult>(new Error("500", "Error while creating statistic"));
    }   
}