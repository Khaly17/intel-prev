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
using Soditech.IntelPrev.Reports.Shared.ReportDatas;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.ReportDatas.Commands;


public class CreateReportDataCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateReportDataCommand, TResult<ReportDataResult>>
{
    private readonly IRepository<ReportData> _reportDataRepository = serviceProvider.GetRequiredService<IRepository<ReportData>>();
    private readonly ILogger<CreateReportDataCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateReportDataCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<ReportDataResult>> Handle(CreateReportDataCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<ReportDataResult>(new Error("400", "cannot create reportData without a tenant"));
            }
            var reportData = _mapper.Map<ReportData>(request);
            reportData.TenantId = _session.TenantId.Value;
            
            reportData.CreatorId = _session.UserId;
            reportData.CreatedAt = DateTimeOffset.UtcNow;

            await _reportDataRepository.AddAsync(reportData, cancellationToken);

            await _publisher.Publish(_mapper.Map<ReportDataCreatedEvent>(reportData), cancellationToken);
            
            return Result.Success(_mapper.Map<ReportDataResult>(reportData));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create reportData");
        }
        
        return Result.Failure<ReportDataResult>(new Error("500", "Error while creating reportData"));
    }   
}