using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.RegisterTypes;

[HttpPost(ReportRoutes.RegisterTypes.Update)]
[Tags("RegisterTypes")]
public class UpdateRegisterTypeEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateRegisterTypeCommand, TResult<RegisterTypeResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<RegisterTypeResult>> HandleAsync(UpdateRegisterTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}