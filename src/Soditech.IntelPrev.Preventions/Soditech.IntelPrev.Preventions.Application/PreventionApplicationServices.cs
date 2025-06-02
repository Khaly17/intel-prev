using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Application;

namespace Soditech.IntelPrev.Preventions.Application;

public static class PreventionApplicationServices
{
    public static IServiceCollection
        AddPreventionServices(this IServiceCollection services)
    {
        services.AddApplicationServices(typeof(PreventionApplicationServices).Assembly);

        return services;
    }
}