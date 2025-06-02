using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;

namespace Soditech.IntelPrev.Mediatheques.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant> 
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.Property(x => x.DisplayName)
            .HasMaxLength(150);
		

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);
        
        builder.Property(a => a.IsDeleted)
            .HasDefaultValue(false);
       
    }
}