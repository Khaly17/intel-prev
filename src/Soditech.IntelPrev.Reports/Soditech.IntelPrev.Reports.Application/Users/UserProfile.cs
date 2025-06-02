using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users.Events;

namespace Soditech.IntelPrev.Reports.Application.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreatedEvent, User>();
        CreateMap<UserUpdatedEvent, User>();
    }
}