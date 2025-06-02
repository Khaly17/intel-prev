using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Shared.Roles;


public record GetRolesQuery : IRequest<TResult<IEnumerable<RoleResult>>>;

public record GetRoleQuery(Guid Id) : IRequest<TResult<RoleResult>>;
public record GetRolesCountQuery : IRequest<TResult<int>>;

public record RoleResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public Guid? TenantId { get; set; } 
    public string? TenantName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;
    
    // users count
    public int UsersCount { get; set; }
}

public record RoleDetailResult
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    
    public string[] Permissions { get; set; } = Array.Empty<string>();
    
    // users
    public string[] Users { get; set; } = Array.Empty<string>();
}

public record GetRoleUsersQuery(Guid Id) : IRequest<TResult<IEnumerable<UserResult>>>;



public record CreateRoleCommand: IRequest<TResult<RoleResult>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? TenantId { get; set; }
    public string Scope { get; set; } = string.Empty;
}
public record UpdateRoleCommand : IRequest<TResult<RoleResult>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? TenantId { get; set; }
    public string Scope { get; set; } = string.Empty;
}

public record DeleteRoleCommand(Guid RoleId) : IRequest<TResult<RoleResult>>;
public record AffectRoleToUserCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? SiteId { get; set; }
}

public record UnAffectRoleToUserCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? SiteId { get; set; }
}