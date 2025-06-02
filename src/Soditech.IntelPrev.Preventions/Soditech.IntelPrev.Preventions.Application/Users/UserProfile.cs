using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users.Events;

namespace Soditech.IntelPrev.Preventions.Application.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreatedEvent, User>();
        CreateMap<UserUpdatedEvent, User>();
    }
}