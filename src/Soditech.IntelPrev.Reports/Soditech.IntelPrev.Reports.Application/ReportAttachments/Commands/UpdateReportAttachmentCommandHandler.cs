using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.Application.ReportAttachments.Commands;

public class UpdateReportAttachmentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateReportAttachmentCommand, TResult<ReportAttachmentResult>>
{
    private readonly IRepository<ReportAttachment> _reportAttachmentRepository = serviceProvider.GetRequiredService<IRepository<ReportAttachment>>();
    private readonly ILogger<UpdateReportAttachmentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateReportAttachmentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<ReportAttachmentResult>> Handle(UpdateReportAttachmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reportAttachment = await _reportAttachmentRepository.GetAsync(request.Id, cancellationToken);
            if (reportAttachment == null)
            {
                return Result.Failure<ReportAttachmentResult>(new Error("404", "ReportAttachment not found"));
            }
            
            _mapper.Map(request, reportAttachment);
            
            reportAttachment.UpdaterId = _session.UserId;
            reportAttachment.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _reportAttachmentRepository.UpdateAsync(reportAttachment, cancellationToken);
            await _publisher.Publish(_mapper.Map<ReportAttachmentUpdatedEvent>(reportAttachment), cancellationToken);

            
            return Result.Success(_mapper.Map<ReportAttachmentResult>(reportAttachment));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating reportAttachment");

            return Result.Failure<ReportAttachmentResult>(new Error("500", "Error while updating reportAttachment"));
        }
    }   
}