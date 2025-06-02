using AutoMapper;
using Soditech.IntelPrev.Preventions.Shared.Floors;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.Floors;

public class FloorProfile : Profile
{
    public FloorProfile()
    {
        CreateMap<FloorCreatedEvent, Floor>();
        CreateMap<FloorUpdatedEvent, Floor>();
    }
}