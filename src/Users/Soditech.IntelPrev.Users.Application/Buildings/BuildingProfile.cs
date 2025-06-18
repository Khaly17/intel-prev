using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Prevensions.Shared.Floors;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Application.Buildings;

public class BuildingProfile : Profile
{
    public BuildingProfile()
    {
        CreateMap<BuildingCreatedEvent, Building>();
        CreateMap<BuildingUpdatedEvent, Building>();


        CreateMap<FloorCreatedEvent, Floor>();
        CreateMap<FloorUpdatedEvent, Floor>();
    }
}