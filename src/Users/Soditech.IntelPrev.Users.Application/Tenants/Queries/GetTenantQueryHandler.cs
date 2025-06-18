using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Users.Application.Tenants.Queries;

public class GetTenantQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetTenantQuery, TResult<TenantResult>>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    
    public async Task<TResult<TenantResult>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetAsync(request.Id, cancellationToken);

        if (tenant == null)
        {
            return Result.Failure<TenantResult>(new Error("404", "Tenant not found"));
        }

        var tenantResult = _mapper.Map<TenantResult>(tenant);

        tenantResult.UsersCount = tenant.Users.Count;
        tenantResult.RolesCount = tenant.Roles.Count;
      

        return Result.Success(tenantResult);
    }
}