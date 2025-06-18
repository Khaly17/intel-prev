using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Soditech.IntelPrev.Users.Persistence;
using Soditech.IntelPrev.Users.Persistence.Seeders;

namespace Soditech.IntelPrev.Users.WebApi.HostedServices;

/// <summary>
/// Creates initial data that is needed to property run the application
/// and make client-to-server communication possible.
/// </summary>
/// <param name="serviceProvider"></param>
public class OpenIddictDataSeed(IServiceProvider serviceProvider) : IHostedService
{
	
	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using var scope = serviceProvider.CreateScope();

		var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
		
		var seeder = scope.ServiceProvider.GetRequiredService<OpenIddictDataSeeder>();
		
		await context.Database.EnsureCreatedAsync(cancellationToken);
		
		await seeder.CreateApplicationsAsync();
		
		//get seedIsEnabled value from configuration. If it is false, return
		if (scope.ServiceProvider.GetRequiredService<IConfiguration>().GetSection("SeedIsEnabled").Value == "false")
		{
			return;
		}
		// var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
		var userSeeder = new UserSeeder();
		await userSeeder.SeedAsync(context);
	}

	public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}