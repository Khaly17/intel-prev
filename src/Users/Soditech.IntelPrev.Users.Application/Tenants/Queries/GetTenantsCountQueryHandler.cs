using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Users.Application.Tenants.Queries;

public class GetTenantsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetTenantsCountQuery, TResult<int>>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly ILogger<GetTenantsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetTenantsCountQueryHandler>>();

    public async Task<TResult<int>> Handle(GetTenantsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tenantsCount = await _tenantRepository.CountAsync(cancellationToken: cancellationToken);
                
            return Result.Success(tenantsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting tenants, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting tenants"));
        }
    }
}