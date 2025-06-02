using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.Application.Reports.Commands;

public class DeleteReportCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteReportCommand, Result>
{
    private readonly IRepository<Report> _reportRepository = serviceProvider.GetRequiredService<IRepository<Report>>();
    private readonly ILogger<DeleteReportCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteReportCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _reportRepository.GetAsync(request.Id, cancellationToken);
            if (report == null)
            {
                return Result.Failure<ReportResult>(new Error("404", "Report not found"));
            }
            
            await _reportRepository.DeleteAsync(report, cancellationToken);
            
            await _publisher.Publish(new ReportDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting report");

            return Result.Failure(new Error("500", "Error while deleting report"));
        }
    }   
}