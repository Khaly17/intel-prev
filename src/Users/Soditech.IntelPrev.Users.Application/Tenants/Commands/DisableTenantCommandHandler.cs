using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Tenants.Events;

namespace Soditech.IntelPrev.Users.Application.Tenants.Commands;

public class DisableTenantCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DisableTenantCommand, Result>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly ILogger<DisableTenantCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DisableTenantCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DisableTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = await _tenantRepository.GetAsync(request.Id, cancellationToken);
            if (tenant == null)
            {
                return Result.Failure<TenantResult>(new Error("404", "Tenant not found"));
            }
            
            tenant.IsActive = false;
            await _tenantRepository.UpdateAsync(tenant, cancellationToken);
            
            await _publisher.Publish(_mapper.Map<TenantUpdatedEvent>(tenant), cancellationToken);
            
            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while disabling tenant");

            return Result.Failure(new Error("500", "Error while disabling tenant"));
        }
    }   
}