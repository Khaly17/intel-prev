using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Reports.Application.Tenants.Events;

public class TenantDeletedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<TenantDeletedEvent>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly ILogger<TenantCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<TenantCreatedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(TenantDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = await  _tenantRepository.GetAsync(notification.Id, cancellationToken);
            if (tenant != null)
            {
                await _tenantRepository.DeleteAsync(tenant, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting tenant");
        }
    }
}