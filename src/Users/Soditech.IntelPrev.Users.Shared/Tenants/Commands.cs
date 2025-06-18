using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Shared.Tenants;

public record TenantResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    
    public string? CreatorFullName { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public string? DeleterFullName { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public string? UpdaterFullName { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    // tenant users
    //public List<UserResult> Users { get; set; } = [];
    public long UsersCount { get; set; }
    public long RolesCount { get; set; }
    public  string AdminEmail { get; set; } = string.Empty;
    public string AdminFirstName { get; set; } = string.Empty;
    public string AdminLastName { get; set; } = string.Empty;
}

public record GetTenantsQuery : IRequest<TResult<IEnumerable<TenantResult>>>;

public record GetTenantQuery(Guid Id) : IRequest<TResult<TenantResult>>;

public record GetUsersTenantQuery(Guid Id) : IRequest<TResult<IEnumerable<UserResult>>>;

public record GetRolesTenantQuery(Guid Id) : IRequest<TResult<IEnumerable<RoleResult>>>;

public record GetTenantsCountQuery : IRequest<TResult<int>>;

public record CreateTenantCommand : IRequest<TResult<TenantResult>>
{
    public required string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public required string AdminEmail { get; set; }
    public required string AdminUsername { get; set; }
    public string AdminFirstName { get; set; } = string.Empty;
    public string AdminLastName { get; set; } = string.Empty;
}

public record UpdateTenantCommand : IRequest<TResult<TenantResult>>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? DisplayName { get; set; }
    public bool? IsActive { get; set; }
}

public record DisableTenantCommand(Guid Id) : IRequest<Result>;

public record EnableTenantCommand(Guid Id) : IRequest<Result>;

public record DeleteTenantCommand(Guid Id) : IRequest<Result>;
