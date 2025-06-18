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
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.Application.Reports.Queries;

public class GetReportsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetReportsQuery, TResult<IEnumerable<ReportResult>>>
{
    private readonly IRepository<Report> _reportRepository = serviceProvider.GetRequiredService<IRepository<Report>>();
    private readonly ILogger<GetReportsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<ReportResult>>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reports = await _reportRepository
                .GetAll
                .Where(report => report.TenantId == _session.TenantId)
                .ToListAsync(cancellationToken);

            var reportResults = _mapper.Map<IEnumerable<ReportResult>>(reports);

            return Result.Success(reportResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting reports, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<ReportResult>>(new Error("500", "Error while getting reports"));
        }
    }
}