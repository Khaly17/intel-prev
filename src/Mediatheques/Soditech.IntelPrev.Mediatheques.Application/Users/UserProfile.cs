using AutoMapper;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users.Events;

namespace Soditech.IntelPrev.Mediatheques.Application.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreatedEvent, User>();
        CreateMap<UserUpdatedEvent, User>();
    }
}