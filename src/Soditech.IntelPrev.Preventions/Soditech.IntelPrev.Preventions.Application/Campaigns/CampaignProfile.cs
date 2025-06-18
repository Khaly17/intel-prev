using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Campaigns;

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