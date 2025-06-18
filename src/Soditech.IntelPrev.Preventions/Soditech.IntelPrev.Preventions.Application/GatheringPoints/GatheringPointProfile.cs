using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.GatheringPoints;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.GatheringPoints;

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