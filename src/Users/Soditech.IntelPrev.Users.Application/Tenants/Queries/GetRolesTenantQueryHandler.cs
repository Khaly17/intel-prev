using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Users.Application.Tenants.Queries;

public class GetRolesTenantQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetRolesTenantQuery, TResult<IEnumerable<RoleResult>>>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    
    public async Task<TResult<IEnumerable<RoleResult>>> Handle(GetRolesTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetAsync(request.Id, cancellationToken);

        if (tenant == null)
        {
            return Result.Failure<IEnumerable<RoleResult>>(new Error("404", "Tenant not found"));
        }

        var usersResult = _mapper.Map<IEnumerable<RoleResult>>(tenant.Roles);

        return Result.Success(usersResult);
    }
}