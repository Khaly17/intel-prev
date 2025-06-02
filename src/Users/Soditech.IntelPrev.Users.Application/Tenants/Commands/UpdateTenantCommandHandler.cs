using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Tenants.Events;

namespace Soditech.IntelPrev.Users.Application.Tenants.Commands;

public class UpdateTenantCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateTenantCommand, TResult<TenantResult>>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly ILogger<UpdateTenantCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateTenantCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
  
    public async Task<TResult<TenantResult>> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = await _tenantRepository.GetAsync(request.Id, cancellationToken);
            if (tenant == null)
            {
                return Result.Failure<TenantResult>(new Error("404", "Tenant not found"));
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                request.Name = request.Name.ToUpper();
                            
                if (tenant.Name != request.Name)
                {
                    // check if the tenant name already exists
                    var existingTenant = await _tenantRepository.GetAll.FirstOrDefaultAsync(t => t.Name == request.Name, cancellationToken: cancellationToken);
                    if (existingTenant != null)
                    {
                        return Result.Failure<TenantResult>(new Error("400", "Tenant with the same name does already exist."));
                    }
                }
            }
            
            _mapper.Map(request, tenant);
            
            await _tenantRepository.UpdateAsync(tenant, cancellationToken);
            
            await _publisher.Publish(_mapper.Map<TenantUpdatedEvent>(tenant), cancellationToken);

            return Result.Success(_mapper.Map<TenantResult>(tenant));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating tenant");

            return Result.Failure<TenantResult>(new Error("500", "Error while updating tenant"));
        }
    }   
}