using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Roles;

[HttpGet(UserRoutes.Roles.GetAll)]
[Tags("Roles")]
public class GetRolesEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<RoleResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<RoleResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetRolesQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}