using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Persistence;
using Soditech.IntelPrev.Reports.Persistence.Seeders;

namespace Soditech.IntelPrev.Reports.Persistence;

public static class ReportPersistenceService
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ReportDbContext>(options =>
        {
            options.UseLazyLoadingProxies();

            options.UseSqlServer(configuration.GetConnectionString("Default"));

        });

        services.AddRepositories<ReportDbContext>();

        services.AddHostedService<ReportSeederHostedService>();

        return services;
    }
}