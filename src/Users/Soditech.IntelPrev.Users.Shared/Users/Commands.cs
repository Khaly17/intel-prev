using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Shared.Users;

public record GetUsersQuery : IRequest<TResult<IEnumerable<UserResult>>>;
public record GetUsersCountQuery : IRequest<TResult<int>>;
public record GetUserNotificationTagsQuery : IRequest<TResult<IEnumerable<string>>>;

public record GetUserInfoQuery: IRequest<TResult<UserResult>>;
public record GetUserQuery(Guid Id) : IRequest<TResult<UserResult>>;
public record UserResult
{
    public Guid Id { get; set; } 
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;   
    public string PhoneNumber { get; set; } = string.Empty;   
    public string Email { get; set; } = string.Empty;   
    
    public string AppVersion { get; set; } = string.Empty;
    public string? SiteName { get; set; }
    public Guid? SiteId { get; set; }
    
    public Guid? BuildingId { get; set; }
    public string? BuildingName { get; set; }
      
    public Guid? FloorId { get; set; }
    public int? FloorNumber { get; set; }
    
    public string? TenantName { get; set; }
    public Guid? TenantId { get; set; }
    
    public int AccessFailedCount { get; set; }
    public IEnumerable<RoleResult> Roles { get; set; } = default!;
    
    // roleCount
    public int RoleCount { get; set; }
}
public record UserResultComparer : IEqualityComparer<UserResult>
{
    public bool Equals(UserResult x, UserResult y) => x.Id == y.Id;

    public int GetHashCode(UserResult obj)
    {
        return obj.Id.GetHashCode();
    }

}


public record UserDetailResult
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;   
    public string AppVersion { get; set; } = string.Empty;   
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    private string[] _roles = [];

    public IReadOnlyList<string> Roles => _roles;
}


public record RemoveUserRoleCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? ApplicationId { get; set; }    
    public Guid? SiteId { get; set; }
}

public record AddUserRoleCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? ApplicationId { get; set; }
    public Guid? SiteId { get; set; }
}
public record CreateUserCommand : IRequest<TResult<UserResult>>
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid? TenantId { get; set; }
    public Guid? BuildId { get; set; }
    public Guid? FloorId { get; set; }
}


public record UpdateUserCommand : IRequest<TResult<UserResult>>
{
    public Guid Id { get; set; } 
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? SiteId { get; set; }
    public int? AccessFailedCount { get; set; }
    public Guid? TenantId { get; set; }
}
public record UpdateUserAppVersionUserCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public required string Version { get; set; }
}

public record RemoveUserCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
}

public record MobileLoginCommand : IRequest<TResult<MobileLoginResult>>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
}

public record MobileLoginResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}

public record UpdatePasswordCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public record DeleteUserCommand(Guid Id) : IRequest<Result>;
public record ResetPasswordCommand(Guid Id) : IRequest<Result>;
public record ForgetPasswordCommand(string UserName) : IRequest<Result>;