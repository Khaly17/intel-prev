using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportComments;

namespace Soditech.IntelPrev.Reports.Application.ReportComments.Commands;

public class UpdateReportCommentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateReportCommentCommand, TResult<ReportCommentResult>>
{
    private readonly IRepository<ReportComment> _reportCommentRepository = serviceProvider.GetRequiredService<IRepository<ReportComment>>();
    private readonly ILogger<UpdateReportCommentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateReportCommentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<ReportCommentResult>> Handle(UpdateReportCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reportComment = await _reportCommentRepository.GetAsync(request.Id, cancellationToken);
            if (reportComment == null)
            {
                return Result.Failure<ReportCommentResult>(new Error("404", "ReportComment not found"));
            }
            
            _mapper.Map(request, reportComment);
            
            reportComment.UpdaterId = _session.UserId;
            reportComment.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _reportCommentRepository.UpdateAsync(reportComment, cancellationToken);
            await _publisher.Publish(_mapper.Map<ReportCommentUpdatedEvent>(reportComment), cancellationToken);

            
            return Result.Success(_mapper.Map<ReportCommentResult>(reportComment));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating reportComment");

            return Result.Failure<ReportCommentResult>(new Error("500", "Error while updating reportComment"));
        }
    }   
}