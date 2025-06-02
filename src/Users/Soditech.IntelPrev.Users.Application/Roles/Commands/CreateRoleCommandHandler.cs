using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Application.Roles.Commands;

public class CreateRoleCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateRoleCommand, TResult<RoleResult>>
{
    private readonly RoleManager<Role> _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
    private readonly ILogger<CreateRoleCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateRoleCommandHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    
    public async Task<TResult<RoleResult>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = _mapper.Map<Role>(request);
        
        role.CreatorId = _session.UserId;

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            return Result.Success(_mapper.Map<RoleResult>(role));
        }
        
        // logs error
        _logger.LogError("Error while creating role, {errors}", result.Errors);
        
        return Result.Failure<RoleResult>(new Error("500", "Error while creating role"));
    }
}