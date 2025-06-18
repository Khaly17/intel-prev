using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Application.Roles.Queries;

public class GetRolesCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRolesCountQuery, TResult<int>>
{
    private readonly IRepository<Role> _repositoryRole = serviceProvider.GetRequiredService<IRepository<Role>>();
    

    public async Task<TResult<int>> Handle(GetRolesCountQuery request, CancellationToken cancellationToken)
    {
        
        var rolesCount = await _repositoryRole
            .CountAsync(cancellationToken: cancellationToken);
        
        return Result.Success(rolesCount);
    }
}