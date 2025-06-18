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
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.Application.ReportAttachments.Queries;

public class GetReportAttachmentsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetReportAttachmentsCountQuery, TResult<int>>
{
    private readonly IRepository<ReportAttachment> _reportAttachmentRepository = serviceProvider.GetRequiredService<IRepository<ReportAttachment>>();
    private readonly ILogger<GetReportAttachmentsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportAttachmentsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetReportAttachmentsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reportAttachmentsCount = await _reportAttachmentRepository
                .GetAll
                .Where(reportAttachment => reportAttachment.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(reportAttachmentsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting reportAttachments, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting reportAttachments"));
        }
    }
}