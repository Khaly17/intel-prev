using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Prevensions.Shared.Floors;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.Buildings;

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