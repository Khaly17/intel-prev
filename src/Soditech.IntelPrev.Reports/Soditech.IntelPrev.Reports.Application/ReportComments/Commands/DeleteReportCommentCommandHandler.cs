using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportComments;

namespace Soditech.IntelPrev.Reports.Application.ReportComments.Commands;

public class DeleteReportCommentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteReportCommentCommand, Result>
{
    private readonly IRepository<ReportComment> _reportCommentRepository = serviceProvider.GetRequiredService<IRepository<ReportComment>>();
    private readonly ILogger<DeleteReportCommentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteReportCommentCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteReportCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reportComment = await _reportCommentRepository.GetAsync(request.Id, cancellationToken);
            if (reportComment == null)
            {
                return Result.Failure<ReportCommentResult>(new Error("404", "ReportComment not found"));
            }
            
            await _reportCommentRepository.DeleteAsync(reportComment, cancellationToken);
            
            await _publisher.Publish(new ReportCommentDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting reportComment");

            return Result.Failure(new Error("500", "Error while deleting reportComment"));
        }
    }   
}