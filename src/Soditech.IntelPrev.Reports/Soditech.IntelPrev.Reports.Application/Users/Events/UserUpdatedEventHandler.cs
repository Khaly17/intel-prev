using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Reports.Application.Users.Events;

public class UserUpdatedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<UserUpdatedEvent>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ILogger<UserCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<UserCreatedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetAsync(notification.Id, cancellationToken);

            if (user == null)
            {
                user = _mapper.Map<User>(notification);
                await _userRepository.AddAsync(user, cancellationToken);
            }
            else
            {
                _mapper.Map(notification, user);
                await _userRepository.UpdateAsync(user, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating user");
        }
    }
}