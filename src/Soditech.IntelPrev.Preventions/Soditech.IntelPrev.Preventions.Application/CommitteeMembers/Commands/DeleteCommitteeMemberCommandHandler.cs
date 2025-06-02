using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.Application.CommitteeMembers.Commands;

public class DeleteCommitteeMemberCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteCommitteeMemberCommand, Result>
{
    private readonly IRepository<CommitteeMember> _committeeMemberRepository = serviceProvider.GetRequiredService<IRepository<CommitteeMember>>();
    private readonly ILogger<DeleteCommitteeMemberCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteCommitteeMemberCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteCommitteeMemberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var committeeMember = await _committeeMemberRepository.GetAsync(request.Id, cancellationToken);
            if (committeeMember == null)
            {
                return Result.Failure<CommitteeMemberResult>(new Error("404", "CommitteeMember not found"));
            }
            
            await _committeeMemberRepository.DeleteAsync(committeeMember, cancellationToken);
            
            await _publisher.Publish(new CommitteeMemberDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting committeeMember");

            return Result.Failure(new Error("500", "Error while deleting committeeMember"));
        }
    }   
}