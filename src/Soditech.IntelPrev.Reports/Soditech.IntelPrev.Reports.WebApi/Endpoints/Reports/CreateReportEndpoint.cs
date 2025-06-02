using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Reports;

[HttpPost(ReportRoutes.Reports.Create)]
[Tags("Reports")]
public class CreateReportEndpoint(IServiceProvider serviceProvider): Endpoint<CreateReportCommand, TResult<ReportResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<ReportResult>> HandleAsync(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}