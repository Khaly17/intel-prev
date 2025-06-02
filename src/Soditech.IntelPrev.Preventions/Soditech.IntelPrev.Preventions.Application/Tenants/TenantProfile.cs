using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants.Events;

namespace Soditech.IntelPrev.Preventions.Application.Tenants;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        CreateMap<TenantCreatedEvent, Tenant>();
        CreateMap<TenantUpdatedEvent, Tenant>();
    }
}