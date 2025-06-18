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
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.Application.ReportDatas.Queries;

public class GetReportDatasQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetReportDatasQuery, TResult<IEnumerable<ReportDataResult>>>
{
    private readonly IRepository<ReportData> _reportDataRepository = serviceProvider.GetRequiredService<IRepository<ReportData>>();
    private readonly ILogger<GetReportDatasQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportDatasQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<ReportDataResult>>> Handle(GetReportDatasQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reportDatas = await _reportDataRepository
                .GetAll
                .Where(reportData => reportData.TenantId == _session.TenantId)
                .ToListAsync(cancellationToken);

            var reportDataResults = _mapper.Map<IEnumerable<ReportDataResult>>(reportDatas);

            return Result.Success(reportDataResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting reportDatas, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<ReportDataResult>>(new Error("500", "Error while getting reportDatas"));
        }
    }
}