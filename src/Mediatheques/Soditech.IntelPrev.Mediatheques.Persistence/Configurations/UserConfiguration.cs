using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;

namespace Soditech.IntelPrev.Mediatheques.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Username).HasMaxLength(150).IsRequired();
        builder.Ignore(x => x.FullName);
        
        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}