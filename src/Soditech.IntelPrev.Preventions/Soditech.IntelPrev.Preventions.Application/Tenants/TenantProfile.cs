using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Prevensions.Application.Tenants;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        CreateMap<TenantCreatedEvent, Tenant>();
        CreateMap<TenantUpdatedEvent, Tenant>();
    }
}