using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.RegisterTypes;

[HttpGet(ReportRoutes.RegisterTypes.GetAll)]
[Tags("RegisterTypes")]
public class GetRegisterTypesEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<RegisterTypeResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<RegisterTypeResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetRegisterTypesQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}