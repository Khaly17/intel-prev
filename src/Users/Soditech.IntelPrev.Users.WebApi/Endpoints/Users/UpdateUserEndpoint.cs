using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Users;

[HttpPost(UserRoutes.Users.Update)]
[Tags("Users")]
public class UpdateUserEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateUserCommand, TResult<UserResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<UserResult>> HandleAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}