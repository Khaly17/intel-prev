using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.Application.CommitteeMembers.Queries;

public class GetCommitteeMemberQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetCommitteeMemberQuery, TResult<CommitteeMemberResult>>
{
    private readonly IRepository<CommitteeMember> _committeeMemberRepository = serviceProvider.GetRequiredService<IRepository<CommitteeMember>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<CommitteeMemberResult>> Handle(GetCommitteeMemberQuery request, CancellationToken cancellationToken)
    {
        var committeeMember = await _committeeMemberRepository.GetAsync(request.Id, cancellationToken);

        if (committeeMember == null)
        {
            return Result.Failure<CommitteeMemberResult>(new Error("404", "CommitteeMember not found"));
        }

        var committeeMemberResult = _mapper.Map<CommitteeMemberResult>(committeeMember);

        return Result.Success(committeeMemberResult);
    }
}