using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Notifications.Shared;
using Soditech.IntelPrev.Notifications.Shared.NotificationHubs;

namespace Soditech.IntelPrev.NotificationHubs.WebApi.Endpoints;

[HttpPost(NotificationRoutes.Requests.Request)]
[Tags("Notifications")]
public class RequestPushEndpoint(IServiceProvider serviceProvider) : Endpoint<RequestPushRequest, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

    public override async Task<Result> HandleAsync(RequestPushRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }

}