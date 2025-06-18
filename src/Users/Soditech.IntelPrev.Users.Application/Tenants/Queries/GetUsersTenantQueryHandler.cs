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
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Application.Tenants.Queries;

public class GetUsersTenantQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetUsersTenantQuery, TResult<IEnumerable<UserResult>>>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    
    public async Task<TResult<IEnumerable<UserResult>>> Handle(GetUsersTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetAsync(request.Id, cancellationToken);

        if (tenant == null)
        {
            return Result.Failure<IEnumerable<UserResult>>(new Error("404", "Tenant not found"));
        }

        var usersResult = _mapper.Map<IEnumerable<UserResult>>(tenant.Users);

        return Result.Success(usersResult);
    }
}