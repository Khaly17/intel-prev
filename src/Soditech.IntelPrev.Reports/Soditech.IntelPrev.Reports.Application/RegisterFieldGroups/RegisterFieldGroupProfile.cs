using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;

namespace Soditech.IntelPrev.Reports.Application.RegisterFieldGroups;

public class RegisterFieldGroupProfile : Profile
{
    public RegisterFieldGroupProfile()
    {
        CreateMap<UpdateRegisterFieldGroupCommand, RegisterFieldGroup>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<RegisterFieldGroup, RegisterFieldGroupResult>();
        CreateMap<CreateRegisterFieldGroupCommand, RegisterFieldGroup>();
        
        CreateMap<RegisterFieldGroup, RegisterFieldGroupCreatedEvent>();
        CreateMap<RegisterFieldGroup, RegisterFieldGroupUpdatedEvent>();
    }
    
}