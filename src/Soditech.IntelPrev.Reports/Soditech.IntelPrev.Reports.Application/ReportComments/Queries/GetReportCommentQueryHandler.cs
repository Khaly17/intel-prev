using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportComments;

namespace Soditech.IntelPrev.Reports.Application.ReportComments.Queries;

public class GetReportCommentQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetReportCommentQuery, TResult<ReportCommentResult>>
{
    private readonly IRepository<ReportComment> _reportCommentRepository = serviceProvider.GetRequiredService<IRepository<ReportComment>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<ReportCommentResult>> Handle(GetReportCommentQuery request, CancellationToken cancellationToken)
    {
        var reportComment = await _reportCommentRepository.GetAsync(request.Id, cancellationToken);

        if (reportComment == null)
        {
            return Result.Failure<ReportCommentResult>(new Error("404", "ReportComment not found"));
        }

        var reportCommentResult = _mapper.Map<ReportCommentResult>(reportComment);

        return Result.Success(reportCommentResult);
    }
}