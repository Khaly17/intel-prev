using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Application.CommitteeMembers.Commands;

public class CreateCommitteeMemberCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateCommitteeMemberCommand, TResult<CommitteeMemberResult>>
{
    private readonly IRepository<CommitteeMember> _committeeMemberRepository = serviceProvider.GetRequiredService<IRepository<CommitteeMember>>();
    private readonly ILogger<CreateCommitteeMemberCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateCommitteeMemberCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<CommitteeMemberResult>> Handle(CreateCommitteeMemberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<CommitteeMemberResult>(new Error("400", "cannot create committeeMember without a tenant"));
            }
            var committeeMember = _mapper.Map<CommitteeMember>(request);
            committeeMember.TenantId = _session.TenantId.Value;
            
            committeeMember.CreatorId = _session.UserId;
            committeeMember.CreatedAt = DateTimeOffset.UtcNow;

            await _committeeMemberRepository.AddAsync(committeeMember, cancellationToken);

            await _publisher.Publish(_mapper.Map<CommitteeMemberCreatedEvent>(committeeMember), cancellationToken);
            
            return Result.Success(_mapper.Map<CommitteeMemberResult>(committeeMember));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create committeeMember");
        }
        
        return Result.Failure<CommitteeMemberResult>(new Error("500", "Error while creating committeeMember"));
    }   
}