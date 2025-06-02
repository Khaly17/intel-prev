using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.Application.CommitteeMembers.Queries;

public class GetCommitteeMembersCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetCommitteeMembersCountQuery, TResult<int>>
{
    private readonly IRepository<CommitteeMember> _committeeMemberRepository = serviceProvider.GetRequiredService<IRepository<CommitteeMember>>();
    private readonly ILogger<GetCommitteeMembersCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetCommitteeMembersCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetCommitteeMembersCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var committeeMembersCount = await _committeeMemberRepository
                .GetAll
                .Where(committeeMember => committeeMember.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(committeeMembersCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting committeeMembers, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting committeeMembers"));
        }
    }
}