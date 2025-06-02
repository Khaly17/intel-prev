using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.Application.ReportDatas.Commands;

public class DeleteReportDataCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteReportDataCommand, Result>
{
    private readonly IRepository<ReportData> _reportDataRepository = serviceProvider.GetRequiredService<IRepository<ReportData>>();
    private readonly ILogger<DeleteReportDataCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteReportDataCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteReportDataCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reportData = await _reportDataRepository.GetAsync(request.Id, cancellationToken);
            if (reportData == null)
            {
                return Result.Failure<ReportDataResult>(new Error("404", "ReportData not found"));
            }
            
            await _reportDataRepository.DeleteAsync(reportData, cancellationToken);
            
            await _publisher.Publish(new ReportDataDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting reportData");

            return Result.Failure(new Error("500", "Error while deleting reportData"));
        }
    }   
}