using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Roles;

[HttpGet(UserRoutes.Roles.Count)]
[Tags("Roles")]
public class GetRolesCountEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<int>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<int>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetRolesCountQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}