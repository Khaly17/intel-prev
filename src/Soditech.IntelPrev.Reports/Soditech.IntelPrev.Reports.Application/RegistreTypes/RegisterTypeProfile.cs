using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.Application.RegistreTypes;

public class RegisterTypeProfile : Profile
{
    public RegisterTypeProfile()
    {
        CreateMap<UpdateRegisterTypeCommand, RegisterType>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<RegisterType, RegisterTypeResult>();
        CreateMap<CreateRegisterTypeCommand, RegisterType>();
        
        CreateMap<RegisterType, RegisterTypeCreatedEvent>();
        CreateMap<RegisterType, RegisterTypeUpdatedEvent>();
    }
    
}