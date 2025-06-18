using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Application.Roles.Commands;

public class UnAffectRoleToUserCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UnAffectRoleToUserCommand, Result>
{
    private readonly IRepository<UserRole> _userRoleRepository = serviceProvider.GetRequiredService<IRepository<UserRole>>();
    private readonly ILogger<UnAffectRoleToUserCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UnAffectRoleToUserCommandHandler>>();
    
    public async Task<Result> Handle(UnAffectRoleToUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userRole = await _userRoleRepository.GetAll
                .FirstOrDefaultAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId && ur.TenantId == request.TenantId, cancellationToken: cancellationToken);

            if (userRole == null)
            {
                return Result.Success();
            }

            await _userRoleRepository.DeleteAsync(userRole, cancellationToken);
            
            //TODO: logout user from all applications ?

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while removing role to user");
            
            return Result.Failure(new Error("500", e.Message));
        }
    }
}