using AutoMapper;
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.MedicalContacts;

public class MedicalContactProfile : Profile
{
    public MedicalContactProfile()
    {
        CreateMap<UpdateMedicalContactCommand, MedicalContact>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<MedicalContact, MedicalContactResult>();
        CreateMap<CreateMedicalContactCommand, MedicalContact>();
        
        CreateMap<MedicalContact, MedicalContactCreatedEvent>();
        CreateMap<MedicalContact, MedicalContactUpdatedEvent>();
    }
    
}