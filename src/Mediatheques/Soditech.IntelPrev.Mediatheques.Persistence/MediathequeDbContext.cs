using Microsoft.EntityFrameworkCore;
using Sensor6ty.Persistence;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;

namespace Soditech.IntelPrev.Mediatheques.Persistence;

public class MediathequeDbContext(DbContextOptions<MediathequeDbContext> options) :  ModuleDbContext(options)
{
    protected override string Schema { get; } = "mediatheques";
    public virtual DbSet<Document> Documents { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }
}