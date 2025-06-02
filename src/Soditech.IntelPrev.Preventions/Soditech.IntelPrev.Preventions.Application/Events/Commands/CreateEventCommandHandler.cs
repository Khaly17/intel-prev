using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Events;

namespace Soditech.IntelPrev.Preventions.Application.Events.Commands;

public class CreateEventCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateEventCommand, TResult<EventResult>>
{
    private readonly IRepository<Event> _eventRepository = serviceProvider.GetRequiredService<IRepository<Event>>();
    private readonly ILogger<CreateEventCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateEventCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<EventResult>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<EventResult>(new Error("400", "cannot create event without a tenant"));
            }
            var @event = _mapper.Map<Event>(request);
            @event.TenantId = _session.TenantId.Value;
            
            @event.CreatorId = _session.UserId;
            @event.CreatedAt = DateTimeOffset.UtcNow;

            await _eventRepository.AddAsync(@event, cancellationToken);

            await _publisher.Publish(_mapper.Map<EventCreatedEvent>(@event), cancellationToken);
            
            return Result.Success(_mapper.Map<EventResult>(@event));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create event");
        }
        
        return Result.Failure<EventResult>(new Error("500", "Error while creating event"));
    }   
}