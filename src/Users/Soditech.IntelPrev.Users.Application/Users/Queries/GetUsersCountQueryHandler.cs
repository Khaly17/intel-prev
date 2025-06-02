using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace  Gefco.AuthServerSoditech.IntelPrev.Users.Application.Users.Commands;

public class GetUsersCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetUsersCountQuery, TResult<int>>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly ILogger<GetUsersCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetUsersCountQueryHandler>>();

  

    public async Task<TResult<int>> Handle(GetUsersCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            //TODO: add a filter by tenant id
            var usersCount = await _userRepository.CountAsync(cancellationToken: cancellationToken);
                
            return Result.Success(usersCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting users, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting users"));
        }
    }
}