using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Equipments;

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