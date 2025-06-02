using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Application.Users.Commands;

public class DeleteUserCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<RemoveUserCommand, Result>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly ILogger<DeleteUserCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteUserCommandHandler>>();

    public async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure<UserResult>(new Error("404", "User not found"));
            }
            
            await _userRepository.DeleteAsync(user, cancellationToken);
            
            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting user");

            return Result.Failure(new Error("500", "Error while deleting user"));
        }
    }   
}