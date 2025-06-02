using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Shared.Reports;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Microsoft.AspNetCore.SignalR;

namespace Soditech.IntelPrev.Reports.Application.Reports.Commands;


public class CreateReportCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateReportCommand, TResult<ReportResult>>
{
    private readonly IRepository<Report> _reportRepository = serviceProvider.GetRequiredService<IRepository<Report>>();
    private readonly ILogger<CreateReportCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateReportCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly IHubContext<NotificationsHub, INotificationClient> _context = serviceProvider.GetRequiredService<IHubContext<NotificationsHub, INotificationClient>>();

    public async Task<TResult<ReportResult>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<ReportResult>(new Error("400", "cannot create report without a tenant"));
            }
           
            var report = _mapper.Map<Report>(request);
            report.TenantId = _session.TenantId.Value;
            
            report.CreatorId = _session.UserId;
            report.CreatedAt = DateTimeOffset.UtcNow;

            foreach (var reportData in report.ReportDatas)
            {
                reportData.CreatedAt = report.CreatedAt;
                reportData.CreatorId = report.CreatorId;
                reportData.TenantId = report.TenantId;
            }

            await _reportRepository.AddAsync(report, cancellationToken);

            await _publisher.Publish(_mapper.Map<ReportCreatedEvent>(report), cancellationToken);

            await _context.Clients.All.ReceiveNotification("Un nouveau signalement a été créé avec succès !");
            return Result.Success(_mapper.Map<ReportResult>(report));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create report");
        }
        
        return Result.Failure<ReportResult>(new Error("500", "Error while creating report"));
    }   
}