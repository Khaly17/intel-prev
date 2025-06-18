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

public class DeleteRoleCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly IRepository<Role> _roleRepository = serviceProvider.GetRequiredService<IRepository<Role>>();
    private readonly ILogger<DeleteRoleCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteRoleCommandHandler>>();

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleRepository.GetAsync(request.RoleId, cancellationToken);

            if (role == null)
            {
                return Result.Failure(new Error("404", "Role not found"));
            }

            await _roleRepository.DeleteAsync(role, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while soft deleting the role with ID {RoleId}", request.RoleId);
            return Result.Failure(new Error("500", "Error while deleting role"));
        }
    }
}