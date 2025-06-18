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
using Soditech.IntelPrev.Reports.Shared.ReportComments;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.ReportComments.Commands;


public class CreateReportCommentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateReportCommentCommand, TResult<ReportCommentResult>>
{
    private readonly IRepository<ReportComment> _reportCommentRepository = serviceProvider.GetRequiredService<IRepository<ReportComment>>();
    private readonly ILogger<CreateReportCommentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateReportCommentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<ReportCommentResult>> Handle(CreateReportCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<ReportCommentResult>(new Error("400", "cannot create reportComment without a tenant"));
            }
            var reportComment = _mapper.Map<ReportComment>(request);
            reportComment.TenantId = _session.TenantId.Value;
            
            reportComment.CreatorId = _session.UserId;
            reportComment.CreatedAt = DateTimeOffset.UtcNow;

            await _reportCommentRepository.AddAsync(reportComment, cancellationToken);

            await _publisher.Publish(_mapper.Map<ReportCommentCreatedEvent>(reportComment), cancellationToken);
            
            return Result.Success(_mapper.Map<ReportCommentResult>(reportComment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create reportComment");
        }
        
        return Result.Failure<ReportCommentResult>(new Error("500", "Error while creating reportComment"));
    }   
}