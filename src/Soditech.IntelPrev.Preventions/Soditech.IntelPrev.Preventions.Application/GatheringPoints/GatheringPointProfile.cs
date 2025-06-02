using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.Application.GatheringPoints;

public class GatheringPointProfile : Profile
{
    public GatheringPointProfile()
    {
        CreateMap<UpdateGatheringPointCommand, GatheringPoint>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<GatheringPoint, GatheringPointResult>();
        CreateMap<CreateGatheringPointCommand, GatheringPoint>();
        
        CreateMap<GatheringPoint, GatheringPointCreatedEvent>();
        CreateMap<GatheringPoint, GatheringPointUpdatedEvent>();
    }
    
}