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
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.Application.Reports.Queries;

public class GetReportsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetReportsCountQuery, TResult<int>>
{
    private readonly IRepository<Report> _reportRepository = serviceProvider.GetRequiredService<IRepository<Report>>();
    private readonly ILogger<GetReportsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetReportsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reportsCount = await _reportRepository
                .GetAll
                .Where(report => report.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(reportsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting reports, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting reports"));
        }
    }
}