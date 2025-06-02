using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Persistence.Configurations;

public class FireSecuritySettingsConfiguration : IEntityTypeConfiguration<FireSecuritySettings>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<FireSecuritySettings> builder)
    {
        builder.ToTable("FireSecuritySettings");

        builder.Property(x => x.DefinitionContent).HasMaxLength(10_000).IsRequired();
        builder.Property(x => x.EvacuationCaseContent).HasMaxLength(10_000);
        builder.Property(x => x.FireConsignsContent).HasMaxLength(10_000);
        builder.Property(x => x.FireMaterialsContent).HasMaxLength(10_000);
        builder.Property(x => x.FireSecurityServiceContent).HasMaxLength(10_000);
        builder.Property(x => x.KnownMyEnterpriseContent).HasMaxLength(10_000);
        
        builder.Property(x => x.IsDefault).HasDefaultValue(false);
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}