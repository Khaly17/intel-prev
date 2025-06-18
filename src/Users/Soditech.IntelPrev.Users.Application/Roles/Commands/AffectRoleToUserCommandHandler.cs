using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Application.Roles.Commands;

public class AffectRoleToUserCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<AffectRoleToUserCommand, Result>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly IRepository<Role> _roleRepository = serviceProvider.GetRequiredService<IRepository<Role>>();
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly IRepository<UserRole> _userRoleRepository = serviceProvider.GetRequiredService<IRepository<UserRole>>();
    private readonly ILogger<AffectRoleToUserCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<AffectRoleToUserCommandHandler>>();
    
    public async Task<Result> Handle(AffectRoleToUserCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var user = await _userRepository.GetAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }

            var role = await _roleRepository.GetAsync(request.RoleId, cancellationToken);
            if (role == null)
            {
                return Result.Failure(new Error("404", "Role not found"));
            }

            if (request.TenantId != null)
            {
                var tenant = await _tenantRepository.GetAsync(request.TenantId.Value, cancellationToken);
                if (tenant == null)
                {
                    return Result.Failure(new Error("404", "Tenant not found"));
                }
            }

            await _userRoleRepository.AddAsync(new UserRole()
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
                TenantId = request.TenantId,
            }, cancellationToken);

            
            return Result.Success();
        }
        catch (Exception e)
        {
            // logs the error
            _logger.LogError(e, "Error while adding role to user");
            
            return Result.Failure(new Error("500", e.Message));
        }
    }
}