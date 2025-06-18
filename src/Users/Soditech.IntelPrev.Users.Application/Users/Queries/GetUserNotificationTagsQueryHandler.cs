using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Notifications.Shared.Models;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Application.Users.Queries;

public class GetUserNotificationTagsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetUserNotificationTagsQuery, TResult<IEnumerable<string>>>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    private readonly IRepository<UserRole> _userRoleRepository = serviceProvider.GetRequiredService<IRepository<UserRole>>();
    private readonly ILogger<GetUserNotificationTagsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetUserNotificationTagsQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();



    public async Task<TResult<IEnumerable<string>>> Handle(GetUserNotificationTagsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if(!_session.UserId.HasValue)
            {
                return Result.Failure<IEnumerable<string>>(new Error("401", "User not authenticated"));
            }

            var user = await _userRepository.GetAsync(_session.UserId.Value, cancellationToken: cancellationToken);

            if (user == null)
            {
                return Result.Failure<IEnumerable<string>>(new Error("404", "User not found"));
            }

            var userRoles = await _userRoleRepository.GetAll
                .Where(x => x.UserId == user.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            var tags = new HashSet<string>
            {
                NotificationTags.All,
                NotificationTags.Users.Replace("$userId", user.Id.ToString())
            };

            //get all user roles and add them to the tags
            foreach (var userRole in userRoles)
            {
                tags.Add(NotificationTags.Roles.Replace("$roleId", userRole.RoleId.ToString()));
            }

            if (user.TenantId != null)
            {
                tags.Add(NotificationTags.TenantUsers.Replace("$tenantId", user.TenantId.ToString()));
            }
            else
            {
                tags.Add(NotificationTags.HostUsers);
            }

            if (user.BuildingId != null)
            {
                tags.Add(NotificationTags.BuildingUsers.Replace("$buildingId", user.BuildingId.ToString()));
            }

            return Result.Success<IEnumerable<string>>(tags);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting user notifications, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<string>>(new Error("500", "Error while getting user notifications"));
        }
    }
}