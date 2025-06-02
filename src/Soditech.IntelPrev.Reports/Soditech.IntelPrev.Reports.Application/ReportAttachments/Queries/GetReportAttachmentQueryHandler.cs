using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.Application.ReportAttachments.Queries;

public class GetReportAttachmentQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetReportAttachmentQuery, TResult<ReportAttachmentResult>>
{
    private readonly IRepository<ReportAttachment> _reportAttachmentRepository = serviceProvider.GetRequiredService<IRepository<ReportAttachment>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<ReportAttachmentResult>> Handle(GetReportAttachmentQuery request, CancellationToken cancellationToken)
    {
        var reportAttachment = await _reportAttachmentRepository.GetAsync(request.Id, cancellationToken);

        if (reportAttachment == null)
        {
            return Result.Failure<ReportAttachmentResult>(new Error("404", "ReportAttachment not found"));
        }

        var reportAttachmentResult = _mapper.Map<ReportAttachmentResult>(reportAttachment);

        return Result.Success(reportAttachmentResult);
    }
}