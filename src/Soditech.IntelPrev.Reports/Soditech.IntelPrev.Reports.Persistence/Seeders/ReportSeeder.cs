using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Sensor6ty.Configurations;
using Sensor6ty.Migration;
using Sensor6ty.System.Reflection;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Reports.Persistence.Seeders;

public class ReportSeeder : ISeeder
{
    // json options for deserialization: camelCase
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new FieldTypeJsonConverter()
        }
    };
    public async Task SeedAsync(DbContext context)
    {
        if (context is ReportDbContext authDbContext)
        {
            var configuration = AppConfigurations.Get(
                                typeof(ReportSeeder).Assembly.GetDirectoryPathOrNull()!,
                                "Development", true);

            var basePath = configuration.GetSection("DATA_DIRECTORY").Value;

            // check if basePath is null or empty
            if (string.IsNullOrEmpty(basePath))
            {
                Console.WriteLine("DATA_DIRECTORY is not set in the configuration file.");
                return;
            }

            Console.WriteLine("Start seeding data ...");
            
            await InsertDataFromFileAsync<ReportDbContext, Tenant>(authDbContext,
                    basePath,
                    "tenants.json",
                    CreateTenantIfNotExistsAsync
            );
            
            await InsertDataFromFileAsync<ReportDbContext, User>(authDbContext,
                basePath,
                "users.json",
                CreateUserIfNotExistsAsync
            );
           
            await InsertDataFromFileAsync<ReportDbContext, RegisterType>(authDbContext,
                basePath,
                "registerTypes.json",
                CreateRegisterTypeIfNotExistsAsync
            );
           

            Console.WriteLine("Seeding completed.");
        }
    }

    // define InsertDataFromFileAsync method
    private static async Task InsertDataFromFileAsync<TDbContext, T>(
        TDbContext context,
        string basePath,
        string fileName,
        Func<TDbContext, T, Task> createIfNotExistsAsync) where TDbContext : DbContext
    {
        try
        {
            Console.WriteLine($"Seeding data for {typeof(T).Name}...");

            var data = await File.ReadAllTextAsync(Path.Combine(basePath, fileName));
            var dataList = JsonSerializer.Deserialize<List<T>>(data, JsonSerializerOptions);

            if (dataList != null)
            {
                foreach (var item in dataList)
                {
                    await createIfNotExistsAsync(context, item);
                }

                await context.SaveChangesAsync();

                Console.WriteLine($"Seed data for {typeof(T).Name} Completed.");
            }
            else
            {
                Console.WriteLine($"No {fileName} data found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to seed data from {fileName}: {ex.Message}");

            Console.WriteLine(ex.InnerException);

            Console.WriteLine($"Seed data for {typeof(T).Name} Not Completed.");

        }
    }
    

    private async Task CreateUserIfNotExistsAsync(ReportDbContext context, User user)
    {
        if (string.IsNullOrEmpty(user.Id.ToString()))
        {
            Console.WriteLine($"User Id {user.Id} does not have a username.");
            return;
        }
        
        if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
        {
            Console.WriteLine($"User Id {user.Id} does not have a username.");
            return;
        }

        try
        {
            var existedUser = await context.Users.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == user.Id || x.Username == user.Username);
            
            if (existedUser == null)
            {
                context.Users.Add(user);
            }
            else
            {
                Console.WriteLine($"User Name {user.Username} or Id {user.Id} already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

   
    // Implement CreateTenantIfNotExistsAsync method
    private async Task CreateTenantIfNotExistsAsync(ReportDbContext context, Tenant tenant)
    {
        if (string.IsNullOrEmpty(tenant.Name))
        {
            Console.WriteLine($"Tenant Id {tenant.Id} does not have a name.");
            return;
        }

        try
        {
            var existedTenant = await context.Tenants.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == tenant.Id || x.Name == tenant.Name);

            if (existedTenant == null)
            {
                context.Tenants.Add(tenant);
            }
            else
            {
                Console.WriteLine($"Tenant Name {tenant.Name} or Id {tenant.Id} already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private async Task CreateRegisterTypeIfNotExistsAsync(ReportDbContext context, RegisterType registerType)
    {
        if (string.IsNullOrEmpty(registerType.Name))
        {
            Console.WriteLine($"RegisterType Id {registerType.Id} does not have a name.");
            return;
        }

        try
        {
            var existedRegisterType = await context.RegisterTypes.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == registerType.Id || x.Name == registerType.Name);

            if (existedRegisterType == null)
            {
                context.RegisterTypes.Add(registerType);
            }
            else
            {
                Console.WriteLine($"RegisterType Name {registerType.Name} or Id {registerType.Id} already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
}