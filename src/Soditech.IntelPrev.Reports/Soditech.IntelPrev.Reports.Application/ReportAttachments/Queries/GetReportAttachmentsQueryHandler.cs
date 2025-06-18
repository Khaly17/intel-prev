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
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.Application.ReportAttachments.Queries;

public class GetReportAttachmentsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetReportAttachmentsQuery, TResult<IEnumerable<ReportAttachmentResult>>>
{
    private readonly IRepository<ReportAttachment> _reportAttachmentRepository = serviceProvider.GetRequiredService<IRepository<ReportAttachment>>();
    private readonly ILogger<GetReportAttachmentsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetReportAttachmentsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<ReportAttachmentResult>>> Handle(GetReportAttachmentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reportAttachments = await _reportAttachmentRepository
                .GetAll
                .Where(reportAttachment => reportAttachment.TenantId == _session.TenantId)
                .ToListAsync(cancellationToken);

            var reportAttachmentResults = _mapper.Map<IEnumerable<ReportAttachmentResult>>(reportAttachments);

            return Result.Success(reportAttachmentResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting reportAttachments, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<ReportAttachmentResult>>(new Error("500", "Error while getting reportAttachments"));
        }
    }
}