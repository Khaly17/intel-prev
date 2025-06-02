using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Application;
using Soditech.IntelPrev.NotificationHubs.Application.Models;
using Soditech.IntelPrev.NotificationHubs.Application.Services;

namespace Soditech.IntelPrev.NotificationHubs.Application;

public static class NotificationApplicationServices
{
    public static IServiceCollection
        AddNotificationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices(typeof(NotificationApplicationServices).Assembly);

        services.AddSingleton<INotificationService, NotificationHubService>();

        services.AddOptions<NotificationHubOptions>()
            .Configure(configuration.GetSection("NotificationHub").Bind)
            .ValidateDataAnnotations();

        return services;
    }
}