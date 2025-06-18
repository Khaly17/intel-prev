using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.Application.ReportAttachments.Commands;

public class DeleteReportAttachmentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteReportAttachmentCommand, Result>
{
    private readonly IRepository<ReportAttachment> _reportAttachmentRepository = serviceProvider.GetRequiredService<IRepository<ReportAttachment>>();
    private readonly ILogger<DeleteReportAttachmentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteReportAttachmentCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteReportAttachmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reportAttachment = await _reportAttachmentRepository.GetAsync(request.Id, cancellationToken);
            if (reportAttachment == null)
            {
                return Result.Failure<ReportAttachmentResult>(new Error("404", "ReportAttachment not found"));
            }
            
            await _reportAttachmentRepository.DeleteAsync(reportAttachment, cancellationToken);
            
            await _publisher.Publish(new ReportAttachmentDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting reportAttachment");

            return Result.Failure(new Error("500", "Error while deleting reportAttachment"));
        }
    }   
}