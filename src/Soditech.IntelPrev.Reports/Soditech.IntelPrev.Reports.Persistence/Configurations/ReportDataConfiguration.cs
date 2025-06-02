using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Persistence.Configurations;

public class ReportDataConfiguration : IEntityTypeConfiguration<ReportData>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ReportData> builder)
    {
        builder.ToTable("ReportDatas");
        builder.Property(x => x.Value).HasMaxLength(1024).IsRequired();
        
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        
        builder.HasOne(u => u.Report)
            .WithMany(x => x.ReportDatas)
            .HasForeignKey(u => u.ReportId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        
        builder.HasOne(u => u.Creator)
            .WithMany()
            .HasForeignKey(u => u.CreatorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Updater)
            .WithMany()
            .HasForeignKey(u => u.UpdaterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(u => u.Deleter)
            .WithMany()
            .HasForeignKey(u => u.DeleterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}