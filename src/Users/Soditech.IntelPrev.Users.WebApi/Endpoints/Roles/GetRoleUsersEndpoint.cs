using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Roles;

[HttpGet(UserRoutes.Roles.GetUsers)]
[Tags("Roles")]
public class GetRoleUsersEndpoint(IServiceProvider serviceProvider): Endpoint<GetRoleUsersQuery, TResult<IEnumerable<UserResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<UserResult>>> HandleAsync(GetRoleUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}