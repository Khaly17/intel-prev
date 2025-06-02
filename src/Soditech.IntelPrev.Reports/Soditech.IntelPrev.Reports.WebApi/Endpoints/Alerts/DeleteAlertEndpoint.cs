using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.Alerts;

[HttpDelete(ReportRoutes.Alerts.Delete)]
[Tags("Alerts")]
public class DeleteAlertEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteAlertCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteAlertCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}