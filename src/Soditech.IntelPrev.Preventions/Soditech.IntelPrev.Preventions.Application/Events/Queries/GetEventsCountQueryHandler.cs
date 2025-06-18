using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.Events;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Events.Queries;

public class GetEventsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetEventsCountQuery, TResult<int>>
{
    private readonly IRepository<Event> _eventRepository = serviceProvider.GetRequiredService<IRepository<Event>>();
    private readonly ILogger<GetEventsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetEventsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetEventsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var eventsCount = await _eventRepository.GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(eventsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting events, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting events"));
        }
    }
}