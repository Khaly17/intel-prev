using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.CommitteeMembers;

[HttpGet(PreventionRoutes.CommitteeMembers.GetAll)]
[Tags("CommitteeMembers")]
public class GetCommitteeMembersEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<CommitteeMemberResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<CommitteeMemberResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetCommitteeMembersQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}