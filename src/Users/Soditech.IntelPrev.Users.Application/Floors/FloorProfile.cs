using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.Floors;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Application.Floors;

public class FloorProfile : Profile
{
    public FloorProfile()
    {
        CreateMap<FloorCreatedEvent, Floor>();
        CreateMap<FloorUpdatedEvent, Floor>();
    }
}