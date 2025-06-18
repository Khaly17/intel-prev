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

[HttpDelete(NotificationRoutes.Installation.Delete)]
[Tags("Notifications")]
public class DeleteInstallationEndpoint(IServiceProvider serviceProvider) : Endpoint<DeleteInstallationRequest, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

    public override async Task<Result> HandleAsync(DeleteInstallationRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}