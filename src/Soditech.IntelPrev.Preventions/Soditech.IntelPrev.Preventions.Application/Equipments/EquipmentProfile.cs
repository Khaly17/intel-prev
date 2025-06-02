using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.Application.Equipments;

public class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<UpdateEquipmentCommand, Equipment>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Equipment, EquipmentResult>();
        CreateMap<CreateEquipmentCommand, Equipment>();
        
        CreateMap<Equipment, EquipmentCreatedEvent>();
        CreateMap<Equipment, EquipmentUpdatedEvent>();
    }
    
}