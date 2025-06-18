using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Prevensions.Shared.Floors;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Buildings;

public class BuildingProfile : Profile
{
    public BuildingProfile()
    {
        CreateMap<UpdateBuildingCommand, Building>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Building, BuildingResult>();
        CreateMap<CreateBuildingCommand, Building>();
        
        CreateMap<Building, BuildingCreatedEvent>();
        CreateMap<Building, BuildingUpdatedEvent>();
        

        CreateMap<CreateFloorCommand, Floor>();
        CreateMap<UpdateFloorCommand, Floor>();

        CreateMap<Floor, FloorCreatedEvent>();
        CreateMap<Floor, FloorUpdatedEvent>();

        CreateMap<Floor, FloorResult>();


    }
    
}