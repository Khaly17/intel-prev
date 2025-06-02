using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Persistence;

namespace Soditech.IntelPrev.Mediatheques.Persistence;

public static class MediathequePersistenceService
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MediathequeDbContext>(options =>
        {
            options.UseLazyLoadingProxies();

            options.UseSqlServer(configuration.GetConnectionString("Default"));

        });

        services.AddRepositories<MediathequeDbContext>();


        return services;
    }
}