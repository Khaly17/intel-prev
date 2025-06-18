using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Application.Users.Commands;

public class UpdateUserAppVersionUserCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateUserAppVersionUserCommand, Result>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly ILogger<UpdateUserAppVersionUserCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateUserAppVersionUserCommandHandler>>();
    
   
    public async Task<Result> Handle(UpdateUserAppVersionUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure(new Error("404", $"User {request.UserId} not found"));
            }
            
            user.AppVersion = request.Version;
            await _userRepository.UpdateAsync(user, cancellationToken);
    
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while removing user");
        }
        
        return Result.Failure(new Error("500", "Error while removing user"));
    }
}