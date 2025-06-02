using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Reports;

[HttpDelete(ReportRoutes.Reports.Delete)]
[Tags("Reports")]
public class DeleteReportEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteReportCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}