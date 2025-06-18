using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sensor6ty.Configurations;
using Sensor6ty.System.Reflection;

namespace Soditech.IntelPrev.Reports.Persistence.Seeders;

public class ReportSeederHostedService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var configuration = AppConfigurations.Get(
                                    typeof(ReportSeederHostedService).Assembly.GetDirectoryPathOrNull()!,
                                    "Development", true);

        //get seedIsEnabled value from configuration. If it is false, return
        if (configuration.GetSection("SeedIsEnabled").Value == "false")
        {
            return;
        }
        
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);
        //ensure apply migrations
        await context.Database.MigrateAsync(cancellationToken);
        
        var seeder = new ReportSeeder();
        await seeder.SeedAsync(context);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}