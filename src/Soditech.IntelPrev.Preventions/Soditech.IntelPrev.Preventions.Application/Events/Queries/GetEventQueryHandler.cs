using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Events;

namespace Soditech.IntelPrev.Preventions.Application.Events.Queries;

public class GetEventQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetEventQuery, TResult<EventResult>>
{
    private readonly IRepository<Event> _eventRepository = serviceProvider.GetRequiredService<IRepository<Event>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    
    public async Task<TResult<EventResult>> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetAsync(request.Id, cancellationToken);

        if (@event == null)
        {
            return Result.Failure<EventResult>(new Error("404", "Event not found"));
        }

        var eventResult = _mapper.Map<EventResult>(@event);

        return Result.Success(eventResult);
    }
}