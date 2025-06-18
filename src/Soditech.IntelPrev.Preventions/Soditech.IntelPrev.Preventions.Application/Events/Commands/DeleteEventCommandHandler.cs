using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Events;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Events.Commands;

public class DeleteEventCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteEventCommand, Result>
{
    private readonly IRepository<Event> _eventRepository = serviceProvider.GetRequiredService<IRepository<Event>>();
    private readonly ILogger<DeleteEventCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteEventCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var @event = await _eventRepository.GetAsync(request.Id, cancellationToken);
            if (@event == null)
            {
                return Result.Failure<EventResult>(new Error("404", "Event not found"));
            }
            
            await _eventRepository.DeleteAsync(@event, cancellationToken);
            
            await _publisher.Publish(new EventDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting event");

            return Result.Failure(new Error("500", "Error while deleting event"));
        }
    }   
}