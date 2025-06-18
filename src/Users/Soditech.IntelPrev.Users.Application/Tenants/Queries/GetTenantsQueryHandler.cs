using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Users.Application.Tenants.Queries;

public class GetTenantsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetTenantsQuery, TResult<IEnumerable<TenantResult>>>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly ILogger<GetTenantsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetTenantsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
  

    public async Task<TResult<IEnumerable<TenantResult>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tenants = await _tenantRepository.GetAllAsync(cancellationToken);

            var tenantResults = _mapper.Map<IEnumerable<TenantResult>>(tenants);

            return Result.Success(tenantResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting tenants, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<TenantResult>>(new Error("500", "Error while getting tenants"));
        }
    }
}