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

public class GetAlertsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetAlertsQuery, TResult<IEnumerable<AlertResult>>>
{
    private readonly IRepository<Alert> _alertRepository = serviceProvider.GetRequiredService<IRepository<Alert>>();
    private readonly ILogger<GetAlertsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetAlertsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<AlertResult>>> Handle(GetAlertsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var alerts = await _alertRepository
                .GetAll
                .Where(alert => alert.TenantId == _session.TenantId)
                .ToListAsync(cancellationToken);

            var alertResults = _mapper.Map<IEnumerable<AlertResult>>(alerts);

            return Result.Success(alertResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting alerts, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<AlertResult>>(new Error("500", "Error while getting alerts"));
        }
    }
}