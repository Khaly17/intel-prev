using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Prevensions.Shared.Events;

public record EventResult
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty;
    
    public Guid OrganizerId { get; set; } 
    public string OrganizerName { get; set; } = default!;
    
    public string? CreatorFullName { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public string? DeleterFullName { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public string? UpdaterFullName { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
}

public record CreateEventCommand : IRequest<TResult<EventResult>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public Guid OrganizerId { get; set; }
}

public record UpdateEventCommand : IRequest<TResult<EventResult>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public Guid OrganizerId { get; set; }
}

public record DeleteEventCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetEventQuery(Guid Id) : IRequest<TResult<EventResult>>;

public record GetEventsQuery : IRequest<TResult<IEnumerable<EventResult>>>;

public record GetUpComingEventsQuery : IRequest<TResult<IEnumerable<EventResult>>>;

public record GetEventsCountQuery : IRequest<TResult<int>>;

