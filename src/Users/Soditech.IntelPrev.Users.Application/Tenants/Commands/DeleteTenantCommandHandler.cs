using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Tenants.Events;

namespace Soditech.IntelPrev.Users.Application.Tenants.Commands;

public class DeleteTenantCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteTenantCommand, Result>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly ILogger<DeleteTenantCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteTenantCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    
    public async Task<Result> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = await _tenantRepository.GetAsync(request.Id, cancellationToken);
            if (tenant == null)
            {
                return Result.Failure<TenantResult>(new Error("404", "Tenant not found"));
            }
            
            await _tenantRepository.DeleteAsync(tenant, cancellationToken);
            
            await _publisher.Publish(new TenantDeletedEvent(tenant.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting tenant");

            return Result.Failure(new Error("500", "Error while deleting tenant"));
        }
    }   
}