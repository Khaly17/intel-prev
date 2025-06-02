using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;

namespace Soditech.IntelPrev.Preventions.Application.Campaigns;

public class CampaignProfile : Profile
{
    public CampaignProfile()
    {
        CreateMap<UpdateCampaignCommand, Campaign>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Campaign, CampaignResult>();
        CreateMap<CreateCampaignCommand, Campaign>();
        
        CreateMap<Campaign, CampaignCreatedEvent>();
        CreateMap<Campaign, CampaignUpdatedEvent>();
    }
    
}