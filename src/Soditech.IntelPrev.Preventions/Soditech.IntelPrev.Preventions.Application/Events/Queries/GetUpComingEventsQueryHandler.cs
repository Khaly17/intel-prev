using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Events;

namespace Soditech.IntelPrev.Preventions.Application.Events.Queries;

public class GetUpComingEventsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetUpComingEventsQuery, TResult<IEnumerable<EventResult>>>
{
    private readonly IRepository<Event> _eventRepository = serviceProvider.GetRequiredService<IRepository<Event>>();
    private readonly ILogger<GetUpComingEventsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetUpComingEventsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<EventResult>>> Handle(GetUpComingEventsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var events = await _eventRepository
                .GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .Where(e => e.EndDate >= DateTimeOffset.UtcNow)
                .OrderBy(e => e.StartDate)
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