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

public class GetCountAlertsGroupedByTypeQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetCountAlertsGroupedByTypeQuery, TResult<IEnumerable<CountAlertsGroupedByTypeResult>>>
{
    private readonly IRepository<Alert> _alertRepository = serviceProvider.GetRequiredService<IRepository<Alert>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetCountAlertsGroupedByTypeQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetCountAlertsGroupedByTypeQueryHandler>>();


    public async Task<TResult<IEnumerable<CountAlertsGroupedByTypeResult>>> Handle(GetCountAlertsGroupedByTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var alerts = await _alertRepository.GetAll
                .Where(alert => alert.TenantId == _session.TenantId)
                .Where(r => r.CreatedAt != null && r.CreatedAt.Value.Date >= request.StartDate && r.CreatedAt.Value.Date <= request.EndDate)
                .GroupBy(r => r.Type)
                .Select(g => new CountAlertsGroupedByTypeResult
                {
                    Type = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);


            var alertsResult = _mapper.Map<IEnumerable<CountAlertsGroupedByTypeResult>>(alerts);

            return Result.Success(alertsResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting alerts.");
        }

        return Result.Failure<IEnumerable<CountAlertsGroupedByTypeResult>>(new Error("500", "Cannot get alerts."));
    }
}