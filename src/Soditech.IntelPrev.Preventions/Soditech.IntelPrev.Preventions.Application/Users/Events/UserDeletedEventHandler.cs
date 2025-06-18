using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Prevensions.Application.Users.Events;

public class UserDeletedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<UserDeletedEvent>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly ILogger<UserCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<UserCreatedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var user = await  _userRepository.GetAsync(notification.Id, cancellationToken);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting user");
        }
    }
}