using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Gefco.AuthServerSoditech.IntelPrev.Users.Application.Users.Commands;

public class GetUserQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetUserQuery, TResult<UserResult>>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly IRepository<UserRole> _userRoleRepository = serviceProvider.GetRequiredService<IRepository<UserRole>>();
    
    public async Task<TResult<UserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.Id, cancellationToken);

        if (user == null)
        {
            return Result.Failure<UserResult>(new Error("404", "User not found"));
        }

        var userRoles = await _userRoleRepository
            .GetAll
            .Where(ur => ur.UserId == user.Id)
            .ToListAsync(cancellationToken);

        var roles = userRoles.Select(ur => new RoleResult()
        {
            Id = ur.RoleId,
            TenantName = ur.Role.Tenant?.Name ?? "Host",
            TenantId = ur.Role.TenantId,
            Name = ur.Role.Name ?? string.Empty,
            Description = ur.Role.Description
        });

        return Result.Success(new UserResult
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            AppVersion = user.AppVersion,
            TenantId = user.TenantId,
            TenantName = user.Tenant?.Name ?? "Host",
            AccessFailedCount = user.AccessFailedCount,
            Roles = roles,
            RoleCount = userRoles.Count,
        });
    }
}