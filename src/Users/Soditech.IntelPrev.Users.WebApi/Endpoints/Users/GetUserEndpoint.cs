using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Users;

[HttpGet(UserRoutes.Users.GetById)]
[Tags("Users")]
public class GetUserEndpoint(IServiceProvider serviceProvider): Endpoint<GetUserQuery, TResult<UserResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<UserResult>> HandleAsync(GetUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}