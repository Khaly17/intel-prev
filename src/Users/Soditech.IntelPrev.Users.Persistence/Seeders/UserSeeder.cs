using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Sensor6ty.Configurations;
using Sensor6ty.Migration;
using Sensor6ty.System.Reflection;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Persistence.EfCore;


public class UserSeeder : ISeeder
{
    // json options for deserialization: camelCase
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    public async Task SeedAsync(DbContext context)
    {
        if (context is UserDbContext authDbContext)
        {

            var configuration = AppConfigurations.Get(Assembly.GetExecutingAssembly().GetDirectoryPathOrNull()!,
                "Development", true);
            var basePath = configuration.GetSection("DATA_DIRECTORY").Value;

            // check if basePath is null or empty
            if (string.IsNullOrEmpty(basePath))
            {
                Console.WriteLine("DATA_DIRECTORY is not set in the configuration file.");
                return;
            }

            Console.WriteLine("Start seeding data ...");
            
            await InsertDataFromFileAsync<UserDbContext, Tenant>(authDbContext,
                    basePath,
                    "tenants.json",
                    CreateTenantIfNotExistsAsync
            );
            
            await InsertDataFromFileAsync<UserDbContext, User>(authDbContext,
                basePath,
                "users.json",
                CreateUserIfNotExistsAsync
            );
           

            await InsertDataFromFileAsync<UserDbContext, Role>(authDbContext,
                basePath,
                "roles.json",
                CreateRoleIfNotExistsAsync
            );

            await InsertDataFromFileAsync<UserDbContext, UserRole>(authDbContext,
                basePath,
                "usersRoles.json",
                CreateUserRoleIfNotExistsAsync
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

    private async Task CreateRoleIfNotExistsAsync(UserDbContext context, Role role)
    {
        if (string.IsNullOrEmpty(role.Name))
        {
            Console.WriteLine($"Role Id {role.Id} does not have a name.");
            return;
        }

        try
        {
            var existedRole = await context.Roles.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == role.Id || x.Name == role.Name);

            if (existedRole == null)
            {
                context.Roles.Add(role);
            }
            else
            {
                Console.WriteLine($"Role Name {role.Name} or Id {role.Id} already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task CreateUserIfNotExistsAsync(UserDbContext context, User user)
    {
        if (string.IsNullOrEmpty(user.UserName))
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
                .FirstOrDefaultAsync(x => x.Id == user.Id || x.UserName == user.UserName);
            
            if (existedUser == null)
            {
                context.Users.Add(user);
            }
            else
            {
                Console.WriteLine($"User Name {user.UserName} or Id {user.Id} already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    // implement CreateUserRoleIfNotExistsAsync method
    private async Task CreateUserRoleIfNotExistsAsync(UserDbContext context, UserRole userRole)
    {
        if (userRole.UserId == Guid.Empty || userRole.RoleId == Guid.Empty)
        {
            Console.WriteLine($"User Id {userRole.UserId} or Role Id {userRole.RoleId} .");
            return;
        }

        try
        {
            // get role
            var role = await context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
            if (role == null)
            {
                Console.WriteLine($"Role {userRole.RoleId} does not exists.");
                return;
            }
            
            var user = await context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Id == userRole.UserId);
            if (user == null)
            {
                Console.WriteLine($"User {userRole.UserId} does not exists.");
                return;
            }

            var existedUserRole = await context.UserRoles.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId);

            if (existedUserRole == null)
            {
                context.UserRoles.Add(userRole);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    // Implement CreateTenantIfNotExistsAsync method
    private async Task CreateTenantIfNotExistsAsync(UserDbContext context, Tenant tenant)
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
    
}
