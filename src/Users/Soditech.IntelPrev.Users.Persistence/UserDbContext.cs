using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Persistence;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
{
	protected string Schema { get; } = "users";
	public virtual DbSet<Tenant> Tenants { get; set; }
	public virtual DbSet<Building> Buildings { get; set; }
	//public virtual DbSet<Floor> Floors { get; set; }
	

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		if (!string.IsNullOrWhiteSpace(Schema))
			builder.HasDefaultSchema(Schema);

		builder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

		// Configure OpenIddict tables
		builder.UseOpenIddict();
	}
}