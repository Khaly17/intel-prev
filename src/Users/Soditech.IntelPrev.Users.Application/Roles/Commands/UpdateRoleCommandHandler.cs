using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Application.Roles.Commands;

public class UpdateRoleCommandHandler(
    IServiceProvider serviceProvider) : IRequestHandler<UpdateRoleCommand, TResult<RoleResult>>
{
    private readonly RoleManager<Role> _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
    private readonly ILogger<UpdateRoleCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateRoleCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    
    public async Task<TResult<RoleResult>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
        {
            return Result.Failure<RoleResult>(new Error("404", "Role not found"));
        }

        _mapper.Map(request, role);
        
        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            return Result.Success(_mapper.Map<RoleResult>(role));
        }
        
        _logger.LogError("Error while updating role, {errors}", result.Errors);

        return Result.Failure<RoleResult>(new Error("500", "Error while updating role"));
    }   
}