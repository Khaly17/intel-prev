using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Roles;

[HttpGet(UserRoutes.Roles.GetById)]
[Tags("Roles")]
public class GetRoleEndpoint(IServiceProvider serviceProvider): Endpoint<GetRoleQuery, TResult<RoleResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<RoleResult>> HandleAsync(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}