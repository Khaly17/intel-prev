using Microsoft.EntityFrameworkCore;
using Sensor6ty.Persistence;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Persistence;


public class PreventionDbContext(DbContextOptions<PreventionDbContext> options) :  ModuleDbContext(options)
{
    protected override string Schema { get; } = "preventions";
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<Campaign> Campaigns { get; set; }
    public virtual DbSet<Building> Buildings { get; set; }
    public virtual DbSet<Equipment> Equipments { get; set; }
    public virtual DbSet<GeoLocation> GeoLocations { get; set; }
    public virtual DbSet<GatheringPoint> GatheringPoints { get; set; }
    public virtual DbSet<Statistic> Statistics { get; set; }
    public virtual DbSet<CommitteeMember> CommitteeMembers { get; set; }
    public virtual DbSet<MedicalContact> MedicalContacts { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<StaticContent> StaticContents { get; set; }
    public virtual DbSet<PreventionSettings> PreventionSettings { get; set; }
    public virtual DbSet<FireSecuritySettings> FireSecuritySettings { get; set; }
    public virtual DbSet<ProPrevSettings> ProPrevSettings { get; set; }
    
}