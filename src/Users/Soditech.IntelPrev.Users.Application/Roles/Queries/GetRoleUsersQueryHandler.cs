using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Application.Roles.Queries;

public class GetRoleUsersQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRoleUsersQuery, TResult<IEnumerable<UserResult>>>
{
    private readonly IRepository<Role> _roleRepository = serviceProvider.GetRequiredService<IRepository<Role>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<IEnumerable<UserResult>>> Handle(GetRoleUsersQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(request.Id, cancellationToken: cancellationToken);
        if (role == null)
        {
            return Result.Failure<IEnumerable<UserResult>>(new Error("404", "Role not found"));
        }

        var usersResult = _mapper.Map<IEnumerable<UserResult>>(role.Users);
        //var usersResult = _mapper.Map<IEnumerable<UserResult>>(role.UserRoles.Select(ur => ur.User));
        return Result.Success(usersResult);
    }
}