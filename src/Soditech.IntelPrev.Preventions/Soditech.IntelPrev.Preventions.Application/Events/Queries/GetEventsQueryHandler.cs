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
using Soditech.IntelPrev.Prevensions.Shared.Events;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Events.Queries;

public class GetEventsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetEventsQuery, TResult<IEnumerable<EventResult>>>
{
    private readonly IRepository<Event> _eventRepository = serviceProvider.GetRequiredService<IRepository<Event>>();
    private readonly ILogger<GetEventsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetEventsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<EventResult>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var events = await _eventRepository
                .GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var eventResults = _mapper.Map<List<EventResult>>(events);

            return Result.Success<IEnumerable<EventResult>>(eventResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting events, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<EventResult>>(new Error("500", "Error while getting events"));
        }
    }
}