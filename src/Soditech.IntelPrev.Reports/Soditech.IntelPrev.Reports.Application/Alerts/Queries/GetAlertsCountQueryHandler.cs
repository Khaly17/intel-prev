using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.Application.Alerts.Queries;

public class GetAlertsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetAlertsCountQuery, TResult<int>>
{
    private readonly IRepository<Alert> _alertRepository = serviceProvider.GetRequiredService<IRepository<Alert>>();
    private readonly ILogger<GetAlertsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetAlertsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetAlertsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var alertsCount = await _alertRepository
                .GetAll
                .Where(alert => alert.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(alertsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting alerts, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting alerts"));
        }
    }
}