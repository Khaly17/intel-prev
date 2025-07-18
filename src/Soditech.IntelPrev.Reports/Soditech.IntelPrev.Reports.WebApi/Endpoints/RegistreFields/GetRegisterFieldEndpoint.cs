using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.WebApi.Endpoints.RegistreFields;

[HttpGet(ReportRoutes.RegisterFields.GetById)]
[Tags("RegisterFields")]
public class GetRegisterFieldEndpoint(IServiceProvider serviceProvider): Endpoint<GetRegisterFieldQuery, TResult<RegisterFieldResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<RegisterFieldResult>> HandleAsync(GetRegisterFieldQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}