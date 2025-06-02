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

public class GetCountReportsGroupedByRegisterQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetCountReportsGroupedByRegisterQuery, TResult<IEnumerable<CountReportsGroupedByRegisterResult>>>
{
    private readonly IRepository<Report> _reportRepository = serviceProvider.GetRequiredService<IRepository<Report>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetReportsGroupedByRegisterQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportsGroupedByRegisterQueryHandler>>();

    public async Task<TResult<IEnumerable<CountReportsGroupedByRegisterResult>>> Handle(GetCountReportsGroupedByRegisterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reports = await _reportRepository.GetAll
                .Where(report => report.TenantId == _session.TenantId)
                .Where(r => r.CreatedAt != null && r.CreatedAt.Value.Date >= request.StartDate && r.CreatedAt.Value.Date <= request.EndDate)
                .GroupBy(r => r.RegisterType.Name)
                .Select(g => new CountReportsGroupedByRegisterResult
                {
                    RegisterTypeName = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);

            var reportsResult = _mapper.Map<IEnumerable<CountReportsGroupedByRegisterResult>>(reports);

            return Result.Success(reportsResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting reports.");
        }

        return Result.Failure<IEnumerable<CountReportsGroupedByRegisterResult>>(new Error("500", "Cannot get reports."));
    }
}