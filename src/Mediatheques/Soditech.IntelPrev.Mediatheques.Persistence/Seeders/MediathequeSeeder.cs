using Microsoft.EntityFrameworkCore;
using Sensor6ty.Migration;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;

namespace Soditech.IntelPrev.Mediatheques.Persistence.Seeders;

public class MediathequeSeeder : ISeeder
{
    /// <inheritdoc />
    public async Task SeedAsync(DbContext context)
    {
        if (context is MediathequeDbContext authDbContext)
        {
            var tenant = new Tenant
            {
                Name = "AI4SENSE",
                DisplayName = "Ai4sense",
                IsActive = true
            };
            authDbContext.Tenants.Add(tenant);
            await authDbContext.SaveChangesAsync();
        }
    }
}