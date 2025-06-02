using MediatR;
using Sensor6ty.Results;
using System.ComponentModel.DataAnnotations;

namespace Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

public record CommitteeMemberResult
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string[] Roles { get; set; } = default!;

    public string? CreatorFullName { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public string? DeleterFullName { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? UpdaterFullName { get; set; }

    public Guid? UpdaterId { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}

public record CreateCommitteeMemberCommand : IRequest<TResult<CommitteeMemberResult>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string[] Roles { get; set; } = default!;
}

public record UpdateCommitteeMemberCommand : IRequest<TResult<CommitteeMemberResult>>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string[] Roles { get; set; } = default!;
}

public record DeleteCommitteeMemberCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetCommitteeMemberQuery : IRequest<TResult<CommitteeMemberResult>>
{
    public Guid Id { get; set; }
}

public record GetCommitteeMembersQuery : IRequest<TResult<IEnumerable<CommitteeMemberResult>>>;

public record GetCommitteeMembersCountQuery : IRequest<TResult<int>>;