using AutoMapper;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Tenants.Events;

namespace Soditech.IntelPrev.Users.Application.Tenants;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        CreateMap<Tenant, TenantResult>();

        CreateMap<CreateTenantCommand, Tenant>();
        
        CreateMap<UpdateTenantCommand, Tenant>()
            .ForAllMembers(opts => opts
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Tenant, TenantCreatedEvent>();
        CreateMap<Tenant, TenantUpdatedEvent>();
    }
}