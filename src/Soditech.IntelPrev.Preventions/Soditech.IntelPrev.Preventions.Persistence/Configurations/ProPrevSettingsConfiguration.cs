using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Persistence.Configurations;

public class ProPrevSettingsConfiguration : IEntityTypeConfiguration<ProPrevSettings>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ProPrevSettings> builder)
    {
        builder.ToTable("ProPrevSettings");

        builder.Property(x => x.ActionsOrganizerContent).HasMaxLength(10_000).IsRequired();
        builder.Property(x => x.AnalysisToolsContent).HasMaxLength(10_000);
        builder.Property(x => x.CseAgendaContent).HasMaxLength(10_000);
        builder.Property(x => x.DataSheetContent).HasMaxLength(10_000);
        builder.Property(x => x.EpiControlContent).HasMaxLength(10_000);
        builder.Property(x => x.FirstAidKitContent).HasMaxLength(10_000);
        builder.Property(x => x.HealthFormationContent).HasMaxLength(10_000);
        builder.Property(x => x.MyLibraryContent).HasMaxLength(10_000);
        builder.Property(x => x.RiskAnalysisProtocolContent).HasMaxLength(10_000);
        builder.Property(x => x.SecurityQuarterContent).HasMaxLength(10_000);
        builder.Property(x => x.SitesVisitContent).HasMaxLength(10_000);
        
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