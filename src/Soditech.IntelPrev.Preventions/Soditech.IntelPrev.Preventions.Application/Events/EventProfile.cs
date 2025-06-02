using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Events;

namespace Soditech.IntelPrev.Preventions.Application.Events;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<UpdateEventCommand, Event>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Event, EventResult>();
        CreateMap<CreateEventCommand, Event>();
        
        CreateMap<Event, EventCreatedEvent>();
        CreateMap<Event, EventUpdatedEvent>();
    }
    
}