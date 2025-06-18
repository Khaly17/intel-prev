using AutoMapper;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Mediatheques.Application.Tenants;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        CreateMap<TenantCreatedEvent, Tenant>();
        CreateMap<TenantUpdatedEvent, Tenant>();
    }
}