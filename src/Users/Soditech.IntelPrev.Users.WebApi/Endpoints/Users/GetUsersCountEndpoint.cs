using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Users;


[HttpGet(UserRoutes.Users.Count)]
[Tags("Users")]
public class GetUsersCountEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<int>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<int>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetUsersCountQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}