using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

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