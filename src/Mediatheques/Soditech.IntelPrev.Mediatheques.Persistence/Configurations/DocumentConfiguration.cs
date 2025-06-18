using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared.Enums;

namespace Soditech.IntelPrev.Mediatheques.Persistence.Configurations;


public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Path).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(255);
        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.Property(x => x.Type)
            .HasConversion(p => p.ToString(),
            p => (DocumentType)Enum.Parse(typeof(DocumentType), p))
            .IsRequired();
        
        builder.Property(x => x.FileType)
            .HasConversion(p => p.ToString(),
            p => (FileType)Enum.Parse(typeof(FileType), p))
            .IsRequired();
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId);
        
        builder.Property(x => x.IsDownloadable)
            .HasDefaultValue(true);
        
        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(r => r.Creator)
            .WithMany()
            .HasForeignKey(r => r.CreatorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.Updater)
            .WithMany()
            .HasForeignKey(r => r.UpdaterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.Deleter)
            .WithMany()
            .HasForeignKey(r => r.DeleterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
    }
}