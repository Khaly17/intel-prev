using AutoMapper;
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

public class GetReportCommentsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetReportCommentsQuery, TResult<IEnumerable<ReportCommentResult>>>
{
    private readonly IRepository<ReportComment> _reportCommentRepository = serviceProvider.GetRequiredService<IRepository<ReportComment>>();
    private readonly ILogger<GetReportCommentsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportCommentsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<ReportCommentResult>>> Handle(GetReportCommentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reportComments = await _reportCommentRepository
                .GetAll
                .Where(reportComment => reportComment.TenantId == _session.TenantId)
                .ToListAsync(cancellationToken);

            var reportCommentResults = _mapper.Map<IEnumerable<ReportCommentResult>>(reportComments);

            return Result.Success(reportCommentResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting reportComments, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<ReportCommentResult>>(new Error("500", "Error while getting reportComments"));
        }
    }
}