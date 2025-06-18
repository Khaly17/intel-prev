using AutoMapper;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Application.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UpdateUserCommand, User>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<User, UserResult>();
        CreateMap<CreateUserCommand, User>();
        
        CreateMap<User, UserCreatedEvent>();
        CreateMap<User, UserUpdatedEvent>();
    }
}