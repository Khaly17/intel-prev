using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.Application.ReportDatas.Queries;

public class GetReportDataQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetReportDataQuery, TResult<ReportDataResult>>
{
    private readonly IRepository<ReportData> _reportDataRepository = serviceProvider.GetRequiredService<IRepository<ReportData>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<ReportDataResult>> Handle(GetReportDataQuery request, CancellationToken cancellationToken)
    {
        var reportData = await _reportDataRepository.GetAsync(request.Id, cancellationToken);

        if (reportData == null)
        {
            return Result.Failure<ReportDataResult>(new Error("404", "ReportData not found"));
        }

        var reportDataResult = _mapper.Map<ReportDataResult>(reportData);

        return Result.Success(reportDataResult);
    }
}