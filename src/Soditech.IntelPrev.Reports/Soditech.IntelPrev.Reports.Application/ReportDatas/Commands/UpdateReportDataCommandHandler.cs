using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.Application.ReportDatas.Commands;

public class UpdateReportDataCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateReportDataCommand, TResult<ReportDataResult>>
{
    private readonly IRepository<ReportData> _reportDataRepository = serviceProvider.GetRequiredService<IRepository<ReportData>>();
    private readonly ILogger<UpdateReportDataCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateReportDataCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<ReportDataResult>> Handle(UpdateReportDataCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reportData = await _reportDataRepository.GetAsync(request.Id, cancellationToken);
            if (reportData == null)
            {
                return Result.Failure<ReportDataResult>(new Error("404", "ReportData not found"));
            }
            
            _mapper.Map(request, reportData);
            
            reportData.UpdaterId = _session.UserId;
            reportData.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _reportDataRepository.UpdateAsync(reportData, cancellationToken);
            await _publisher.Publish(_mapper.Map<ReportDataUpdatedEvent>(reportData), cancellationToken);

            
            return Result.Success(_mapper.Map<ReportDataResult>(reportData));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating reportData");

            return Result.Failure<ReportDataResult>(new Error("500", "Error while updating reportData"));
        }
    }   
}