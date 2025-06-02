using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Persistence;

namespace Soditech.IntelPrev.Preventions.Persistence;

public static class PreventionPersistenceService
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PreventionDbContext>(options =>
        {
            options.UseLazyLoadingProxies();

            options.UseSqlServer(configuration.GetConnectionString("Default"));

        });

        services.AddRepositories<PreventionDbContext>();


        return services;
    }
}