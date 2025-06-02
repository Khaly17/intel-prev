using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.Application.Reports.Queries;

public class GetReportQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetReportQuery, TResult<ReportResult>>
{
    private readonly IRepository<Report> _reportRepository = serviceProvider.GetRequiredService<IRepository<Report>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<ReportResult>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetAsync(request.Id, cancellationToken);

        if (report == null)
        {
            return Result.Failure<ReportResult>(new Error("404", "Report not found"));
        }

        var reportResult = _mapper.Map<ReportResult>(report);

        return Result.Success(reportResult);
    }
}