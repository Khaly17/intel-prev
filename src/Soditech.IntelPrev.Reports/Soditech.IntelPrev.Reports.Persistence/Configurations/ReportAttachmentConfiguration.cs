using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Persistence.Configurations;

public class ReportAttachmentConfiguration : IEntityTypeConfiguration<ReportAttachment>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ReportAttachment> builder)
    {
        builder.ToTable("ReportAttachments");
        builder.Property(x => x.FileName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.FileType).HasMaxLength(32).IsRequired();
        builder.Property(x => x.FilePath).HasMaxLength(64).IsRequired();
        
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        
        builder.HasOne(u => u.Report)
            .WithMany()
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