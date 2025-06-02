using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.Application.ReportDatas.Queries;

public class GetReportDatasCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetReportDatasCountQuery, TResult<int>>
{
    private readonly IRepository<ReportData> _reportDataRepository = serviceProvider.GetRequiredService<IRepository<ReportData>>();
    private readonly ILogger<GetReportDatasCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportDatasCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetReportDatasCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reportDatasCount = await _reportDataRepository
                .GetAll
                .Where(reportData => reportData.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(reportDatasCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting reportDatas, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting reportDatas"));
        }
    }
}