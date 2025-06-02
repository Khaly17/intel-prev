using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.RegisterTypes;

[HttpDelete(ReportRoutes.RegisterTypes.Delete)]
[Tags("RegisterTypes")]
public class DeleteRegisterTypeEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteRegisterTypeCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteRegisterTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}