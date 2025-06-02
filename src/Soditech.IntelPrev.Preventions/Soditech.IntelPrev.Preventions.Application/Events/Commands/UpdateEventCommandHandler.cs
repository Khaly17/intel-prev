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

public class UpdateEventCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateEventCommand, TResult<EventResult>>
{
    private readonly IRepository<Event> _eventRepository = serviceProvider.GetRequiredService<IRepository<Event>>();
    private readonly ILogger<UpdateEventCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateEventCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<EventResult>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var @event = await _eventRepository.GetAsync(request.Id, cancellationToken);
            if (@event == null)
            {
                return Result.Failure<EventResult>(new Error("404", "Event not found"));
            }
            
            _mapper.Map(request, @event);
            
            @event.UpdaterId = _session.UserId;
            @event.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _eventRepository.UpdateAsync(@event, cancellationToken);
            await _publisher.Publish(_mapper.Map<EventUpdatedEvent>(@event), cancellationToken);

            
            return Result.Success(_mapper.Map<EventResult>(@event));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating event");

            return Result.Failure<EventResult>(new Error("500", "Error while updating event"));
        }
    }   
}