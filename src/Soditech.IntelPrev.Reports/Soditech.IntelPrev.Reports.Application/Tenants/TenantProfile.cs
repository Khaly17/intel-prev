using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants.Events;

namespace Soditech.IntelPrev.Reports.Application.Tenants;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        CreateMap<TenantCreatedEvent, Tenant>();
        CreateMap<TenantUpdatedEvent, Tenant>();
    }
}