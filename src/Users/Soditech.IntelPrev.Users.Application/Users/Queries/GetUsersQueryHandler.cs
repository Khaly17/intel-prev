using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace  Gefco.AuthServerSoditech.IntelPrev.Users.Application.Users.Commands;

public class GetUsersQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetUsersQuery, TResult<IEnumerable<UserResult>>>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly ILogger<GetUsersQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetUsersQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
  

    public async Task<TResult<IEnumerable<UserResult>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);

            var userResults = _mapper.Map<IEnumerable<UserResult>>(users);

            return Result.Success(userResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting users, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<UserResult>>(new Error("500", "Error while getting users"));
        }
    }
}