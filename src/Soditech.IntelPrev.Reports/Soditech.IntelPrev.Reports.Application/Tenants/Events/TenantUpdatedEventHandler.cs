using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Reports.Application.Tenants.Events;

public class TenantUpdatedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<TenantUpdatedEvent>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ILogger<TenantCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<TenantCreatedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(TenantUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = await _tenantRepository.GetAsync(notification.Id, cancellationToken);

            if (tenant == null)
            {
                tenant = _mapper.Map<Tenant>(notification);
                await _tenantRepository.AddAsync(tenant, cancellationToken);
            }
            else
            {
                _mapper.Map(notification, tenant);
                await _tenantRepository.UpdateAsync(tenant, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating tenant");
        }
    }
}