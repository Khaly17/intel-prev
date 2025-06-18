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
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.ReportAttachments.Commands;


public class CreateReportAttachmentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateReportAttachmentCommand, TResult<ReportAttachmentResult>>
{
    private readonly IRepository<ReportAttachment> _reportAttachmentRepository = serviceProvider.GetRequiredService<IRepository<ReportAttachment>>();
    private readonly ILogger<CreateReportAttachmentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateReportAttachmentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<ReportAttachmentResult>> Handle(CreateReportAttachmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<ReportAttachmentResult>(new Error("400", "cannot create reportAttachment without a tenant"));
            }
            var reportAttachment = _mapper.Map<ReportAttachment>(request);
            reportAttachment.TenantId = _session.TenantId.Value;
            
            reportAttachment.CreatorId = _session.UserId;
            reportAttachment.CreatedAt = DateTimeOffset.UtcNow;

            await _reportAttachmentRepository.AddAsync(reportAttachment, cancellationToken);

            await _publisher.Publish(_mapper.Map<ReportAttachmentCreatedEvent>(reportAttachment), cancellationToken);
            
            return Result.Success(_mapper.Map<ReportAttachmentResult>(reportAttachment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create reportAttachment");
        }
        
        return Result.Failure<ReportAttachmentResult>(new Error("500", "Error while creating reportAttachment"));
    }   
}