using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.CommitteeMembers;

[HttpPost(PreventionRoutes.CommitteeMembers.Update)]
[Tags("CommitteeMembers")]
public class UpdateCommitteeMemberEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateCommitteeMemberCommand, TResult<CommitteeMemberResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<CommitteeMemberResult>> HandleAsync(UpdateCommitteeMemberCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}