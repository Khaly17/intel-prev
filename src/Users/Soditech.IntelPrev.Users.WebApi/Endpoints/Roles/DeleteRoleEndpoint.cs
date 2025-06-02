using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Roles;

[HttpDelete(UserRoutes.Roles.Delete)]
[Tags("Roles")]
public class DeleteRoleEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteRoleCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}