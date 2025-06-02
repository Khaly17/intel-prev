using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.RegisterFields;

[HttpGet(ReportRoutes.RegisterFields.GetAll)]
[Tags("RegisterFields")]
public class GetRegisterFieldsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<RegisterFieldResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<RegisterFieldResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetRegisterFieldsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}