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
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.Application.Reports.Commands;

public class UpdateReportCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateReportCommand, TResult<ReportResult>>
{
    private readonly IRepository<Report> _reportRepository = serviceProvider.GetRequiredService<IRepository<Report>>();
    private readonly ILogger<UpdateReportCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateReportCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<ReportResult>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _reportRepository.GetAsync(request.Id, cancellationToken);
            if (report == null)
            {
                return Result.Failure<ReportResult>(new Error("404", "Report not found"));
            }
            
            _mapper.Map(request, report);
            
            report.UpdaterId = _session.UserId;
            report.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _reportRepository.UpdateAsync(report, cancellationToken);
            await _publisher.Publish(_mapper.Map<ReportUpdatedEvent>(report), cancellationToken);

            
            return Result.Success(_mapper.Map<ReportResult>(report));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating report");

            return Result.Failure<ReportResult>(new Error("500", "Error while updating report"));
        }
    }   
}