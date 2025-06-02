using AutoMapper;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Application.Roles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<CreateRoleCommand, Role>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<UpdateRoleCommand, Role>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Role, RoleResult>();
    }
}