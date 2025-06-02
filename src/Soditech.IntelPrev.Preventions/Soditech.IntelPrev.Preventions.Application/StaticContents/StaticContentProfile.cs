using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.StaticContents;

namespace Soditech.IntelPrev.Preventions.Application.StaticContents;

public class StaticContentProfile : Profile
{
    public StaticContentProfile()
    {
        CreateMap<UpdateStaticContentCommand, StaticContent>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<StaticContent, StaticContentResult>();
        CreateMap<CreateStaticContentCommand, StaticContent>();
        
        CreateMap<StaticContent, StaticContentCreatedEvent>();
        CreateMap<StaticContent, StaticContentUpdatedEvent>();
    }
    
}