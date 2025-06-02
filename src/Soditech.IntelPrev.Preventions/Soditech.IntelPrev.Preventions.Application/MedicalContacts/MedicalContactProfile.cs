using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Preventions.Application.MedicalContacts;

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