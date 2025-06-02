using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.CommitteeMembers;

[HttpGet(PreventionRoutes.CommitteeMembers.GetById)]
[Tags("CommitteeMembers")]
public class GetCommitteeMemberEndpoint(IServiceProvider serviceProvider): Endpoint<GetCommitteeMemberQuery, TResult<CommitteeMemberResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<CommitteeMemberResult>> HandleAsync(GetCommitteeMemberQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}