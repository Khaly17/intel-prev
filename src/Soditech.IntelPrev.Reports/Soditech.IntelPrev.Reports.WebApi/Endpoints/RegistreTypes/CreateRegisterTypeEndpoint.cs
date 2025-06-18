using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.RegistreTypes;

[HttpPost(ReportRoutes.RegisterTypes.Create)]
[Tags("RegisterTypes")]
public class CreateRegisterTypeEndpoint(IServiceProvider serviceProvider): Endpoint<CreateRegisterTypeCommand, TResult<RegisterTypeResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<RegisterTypeResult>> HandleAsync(CreateRegisterTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}