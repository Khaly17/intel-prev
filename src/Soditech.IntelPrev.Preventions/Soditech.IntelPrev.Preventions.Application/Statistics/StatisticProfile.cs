using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Statistics;

namespace Soditech.IntelPrev.Preventions.Application.Statistics;

public class StatisticProfile : Profile
{
    public StatisticProfile()
    {
        CreateMap<UpdateStatisticCommand, Statistic>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Statistic, StatisticResult>();
        CreateMap<CreateStatisticCommand, Statistic>();
        
        CreateMap<Statistic, StatisticCreatedEvent>();
        CreateMap<Statistic, StatisticUpdatedEvent>();
    }
    
}