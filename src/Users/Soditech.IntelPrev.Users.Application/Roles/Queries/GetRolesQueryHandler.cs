using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Roles;

namespace Soditech.IntelPrev.Users.Application.Roles.Commands;

public class GetRolesQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRolesQuery, TResult<IEnumerable<RoleResult>>>
{
    private readonly IRepository<Role> _repositoryRole = serviceProvider.GetRequiredService<IRepository<Role>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    public async Task<TResult<IEnumerable<RoleResult>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {

        var roles = await _repositoryRole.GetAllAsync(cancellationToken);

        var rolesResult = _mapper.Map<IEnumerable<RoleResult>>(roles);

        return Result.Success(rolesResult);
    }
}