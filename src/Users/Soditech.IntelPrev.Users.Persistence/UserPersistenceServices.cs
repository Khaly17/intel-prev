using Soditech.IntelPrev.Users.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Persistence;

namespace Soditech.IntelPrev.Users.Persistence;

/// <summary>
/// Allows infrastructure service registrations.
/// </summary>
/// <remarks>
/// Only entities that implement `IEntityBase` can use the repo pattern.
/// </remarks>
public static class UserPersistenceServices
{
	public static IServiceCollection AddPersistenceServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<UserDbContext>(options =>
		{
            options.UseLazyLoadingProxies();

			options.UseSqlServer(configuration.GetConnectionString("Default"));

			options.UseOpenIddict();

        });

        services.AddRepositories<UserDbContext>();


		return services;
	}
}