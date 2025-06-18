using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.CommitteeMembers.Queries;

public class GetCommitteeMembersQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetCommitteeMembersQuery, TResult<IEnumerable<CommitteeMemberResult>>>
{
    private readonly IRepository<CommitteeMember> _committeeMemberRepository = serviceProvider.GetRequiredService<IRepository<CommitteeMember>>();
    private readonly ILogger<GetCommitteeMembersQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetCommitteeMembersQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<CommitteeMemberResult>>> Handle(GetCommitteeMembersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var committeeMembers = await _committeeMemberRepository
                .GetAll
                .Where(committeeMember => committeeMember.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var committeeMemberResults = _mapper.Map<List<CommitteeMemberResult>>(committeeMembers);

            return Result.Success<IEnumerable<CommitteeMemberResult>>(committeeMemberResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting committeeMembers, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<CommitteeMemberResult>>(new Error("500", "Error while getting committeeMembers"));
        }
    }
}