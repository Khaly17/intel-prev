using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Application.Roles.Queries;

public class GetRoleQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRoleQuery, TResult<RoleResult>>
{
    private readonly RoleManager<Role> _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();


    public async Task<TResult<RoleResult>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
        {
            return Result.Failure<RoleResult>(new Error("404", "Role not found"));
        }

        return Result.Success(new RoleResult
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            TenantId = role.TenantId,
            TenantName = role.Tenant?.Name ?? string.Empty,
        });
    }
}