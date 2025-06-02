using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Users.Shared.Users.Events;

namespace Soditech.IntelPrev.Users.Application.Users.Commands;

public class UpdateUserCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateUserCommand, TResult<UserResult>>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly ILogger<UpdateUserCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateUserCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<UserResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetAsync(request.Id, cancellationToken);
            if (user == null)
            {
                return Result.Failure<UserResult>(new Error("404", "User not found"));
            }
            
            _mapper.Map(request, user);
            
            await _userRepository.UpdateAsync(user, cancellationToken);

            await _publisher.Publish(_mapper.Map<UserUpdatedEvent>(user), cancellationToken);

            return Result.Success(_mapper.Map<UserResult>(user));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating user");

            return Result.Failure<UserResult>(new Error("500", "Error while updating user"));
        }
    }   
}