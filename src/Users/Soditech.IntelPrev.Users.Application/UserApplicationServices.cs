using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Application;

namespace Soditech.IntelPrev.Users.Application;

public static class UserApplicationServices
{
    public static IServiceCollection
        AddUserServices(this IServiceCollection services)
    {
        services.AddApplicationServices(typeof(UserApplicationServices).Assembly);
        
        return services;
    }
}