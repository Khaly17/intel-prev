using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Application;

namespace Soditech.IntelPrev.Prevensions.Application;

public static class PreventionApplicationServices
{
    public static IServiceCollection
        AddPreventionServices(this IServiceCollection services)
    {
        services.AddApplicationServices(typeof(PreventionApplicationServices).Assembly);

        return services;
    }
}