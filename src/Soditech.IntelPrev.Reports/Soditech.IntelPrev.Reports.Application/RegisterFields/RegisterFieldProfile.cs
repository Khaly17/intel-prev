using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.Application.RegisterFields;

public class RegisterFieldProfile : Profile
{
    public RegisterFieldProfile()
    {
        CreateMap<UpdateRegisterFieldCommand, RegisterField>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<RegisterField, RegisterFieldResult>();
        CreateMap<CreateRegisterFieldCommand, RegisterField>();
        
        CreateMap<RegisterField, RegisterFieldCreatedEvent>();
        CreateMap<RegisterField, RegisterFieldUpdatedEvent>();
    }
    
}