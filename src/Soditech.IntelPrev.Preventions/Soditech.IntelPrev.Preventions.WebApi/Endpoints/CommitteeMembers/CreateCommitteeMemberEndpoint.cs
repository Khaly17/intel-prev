using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.CommitteeMembers;

[HttpPost(PreventionRoutes.CommitteeMembers.Create)]
[Tags("CommitteeMembers")]
public class CreateCommitteeMemberEndpoint(IServiceProvider serviceProvider): Endpoint<CreateCommitteeMemberCommand, TResult<CommitteeMemberResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<CommitteeMemberResult>> HandleAsync(CreateCommitteeMemberCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}