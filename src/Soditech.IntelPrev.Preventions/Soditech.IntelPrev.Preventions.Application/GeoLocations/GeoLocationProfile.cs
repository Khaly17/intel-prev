using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.GeoLocations;

public class GeoLocationProfile : Profile
{
    public GeoLocationProfile()
    {
        CreateMap<GeoLocation, GeoLocationResult>()
            .ForMember(g => g.Type, 
                opt => opt.MapFrom(g => g.Type.ToString()));
    }
}