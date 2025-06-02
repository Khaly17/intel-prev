using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Users;

[HttpGet(UserRoutes.Users.UserNotificationTags)]
[Tags("Users")]
public class GetUserNotificationTagsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<string>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<string>>> HandleAsync(CancellationToken cancellationToken=default)
    {
        var request = new GetUserNotificationTagsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}