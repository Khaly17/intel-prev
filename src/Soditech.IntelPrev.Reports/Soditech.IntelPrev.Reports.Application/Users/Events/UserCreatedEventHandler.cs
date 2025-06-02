using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users.Events;

namespace Soditech.IntelPrev.Reports.Application.Users.Events;

public class UserCreatedEventHandler(IServiceProvider serviceProvider) : INotificationHandler<UserCreatedEvent>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ILogger<UserCreatedEventHandler> _logger = serviceProvider.GetRequiredService<ILogger<UserCreatedEventHandler>>();
    
    /// <inheritdoc />
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var user = _mapper.Map<User>(notification);
            await _userRepository.AddAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating user");
        }
    }
}