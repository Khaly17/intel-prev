using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.Application.CommitteeMembers.Commands;

public class UpdateCommitteeMemberCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateCommitteeMemberCommand, TResult<CommitteeMemberResult>>
{
    private readonly IRepository<CommitteeMember> _committeeMemberRepository = serviceProvider.GetRequiredService<IRepository<CommitteeMember>>();
    private readonly ILogger<UpdateCommitteeMemberCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateCommitteeMemberCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<CommitteeMemberResult>> Handle(UpdateCommitteeMemberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var committeeMember = await _committeeMemberRepository.GetAsync(request.Id, cancellationToken);
            if (committeeMember == null)
            {
                return Result.Failure<CommitteeMemberResult>(new Error("404", "CommitteeMember not found"));
            }
            
            _mapper.Map(request, committeeMember);
            
            committeeMember.UpdaterId = _session.UserId;
            committeeMember.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _committeeMemberRepository.UpdateAsync(committeeMember, cancellationToken);
            await _publisher.Publish(_mapper.Map<CommitteeMemberUpdatedEvent>(committeeMember), cancellationToken);

            
            return Result.Success(_mapper.Map<CommitteeMemberResult>(committeeMember));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating committeeMember");

            return Result.Failure<CommitteeMemberResult>(new Error("500", "Error while updating committeeMember"));
        }
    }   
}