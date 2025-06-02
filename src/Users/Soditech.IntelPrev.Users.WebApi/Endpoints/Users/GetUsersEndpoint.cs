using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Users;


[HttpGet(UserRoutes.Users.GetUsers)]
[Tags("Users")]
public class GetUsersEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<UserResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<UserResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetUsersQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}