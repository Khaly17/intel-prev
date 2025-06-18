using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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

public class GetAlertsByPeriodQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetAlertsGroupedByTypeQuery, TResult<IEnumerable<AlertResult>>>
{
    private readonly IRepository<Alert> _alertRepository = serviceProvider.GetRequiredService<IRepository<Alert>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetAlertsByPeriodQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetAlertsByPeriodQueryHandler>>();


    public async Task<TResult<IEnumerable<AlertResult>>> Handle(GetAlertsGroupedByTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var alerts = await _alertRepository.GetAll
                .Where(alert => alert.TenantId == _session.TenantId)
                .Where(r => r.CreatedAt >= request.StartDate && r.CreatedAt <= request.EndDate)
                .ToListAsync(cancellationToken);


            var alertsResult = _mapper.Map<IEnumerable<AlertResult>>(alerts);

            return Result.Success(alertsResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting alerts.");
        }

        return Result.Failure<IEnumerable<AlertResult>>(new Error("500", "Cannot get alerts."));
    }
}