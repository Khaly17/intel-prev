using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User> 
{
	/// <inheritdoc />
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");

		builder.Property(x => x.FirstName)
			.HasMaxLength(50);

		builder.Property(x => x.LastName)
			.HasMaxLength(50);
		
		builder.Ignore(u => u.FullName);

		builder.Property(x => x.AppVersion)
			.HasMaxLength(50);
        
		builder.HasOne(u => u.Tenant)
            .WithMany(a=> a.Users)
            .HasForeignKey(u => u.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);


		//builder.HasMany(u => u.Roles)
		//	.WithMany(r => r.Users)
		//	.UsingEntity<UserRole>();

		builder.Property(a => a.IsDeleted)
            .HasDefaultValue(false);
        
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