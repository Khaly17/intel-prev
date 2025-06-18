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

public class GetReportsGroupedByRegisterQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetReportsGroupedByRegisterQuery, TResult<IEnumerable<ReportResult>>>
{
    private readonly IRepository<Report> _reportRepository = serviceProvider.GetRequiredService<IRepository<Report>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetReportsGroupedByRegisterQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportsGroupedByRegisterQueryHandler>>();


    public async Task<TResult<IEnumerable<ReportResult>>> Handle(GetReportsGroupedByRegisterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reports = await _reportRepository.GetAll
                .Where(report => report.TenantId == _session.TenantId)
                .Where(r => r.CreatedAt != null && r.CreatedAt.Value.Date >= request.StartDate && r.CreatedAt.Value.Date <= request.EndDate)
                .ToListAsync(cancellationToken);


            var reportsResult = _mapper.Map<IEnumerable<ReportResult>>(reports);

            return Result.Success(reportsResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting reports.");
        }

        return Result.Failure<IEnumerable<ReportResult>>(new Error("500", "Cannot get reports."));
    }
}