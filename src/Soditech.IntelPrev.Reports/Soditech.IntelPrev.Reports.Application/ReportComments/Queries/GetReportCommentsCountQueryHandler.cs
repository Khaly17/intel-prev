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
using Soditech.IntelPrev.Reports.Shared.ReportComments;

namespace Soditech.IntelPrev.Reports.Application.ReportComments.Queries;

public class GetReportCommentsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetReportCommentsCountQuery, TResult<int>>
{
    private readonly IRepository<ReportComment> _reportCommentRepository = serviceProvider.GetRequiredService<IRepository<ReportComment>>();
    private readonly ILogger<GetReportCommentsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportCommentsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetReportCommentsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reportCommentsCount = await _reportCommentRepository
                .GetAll
                .Where(reportComment => reportComment.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(reportCommentsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting reportComments, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting reportComments"));
        }
    }
}