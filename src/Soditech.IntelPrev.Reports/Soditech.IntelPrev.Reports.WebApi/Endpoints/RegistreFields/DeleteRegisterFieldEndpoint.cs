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

[HttpDelete(ReportRoutes.RegisterFields.Delete)]
[Tags("RegisterFields")]
public class DeleteRegisterFieldEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteRegisterFieldCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteRegisterFieldCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}