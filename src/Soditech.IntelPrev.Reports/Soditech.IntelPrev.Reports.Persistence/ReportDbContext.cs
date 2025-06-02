using Microsoft.EntityFrameworkCore;
using Sensor6ty.Persistence;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Persistence;

public class ReportDbContext(DbContextOptions<ReportDbContext> options) :  ModuleDbContext(options)
{
    protected override string Schema { get; } = "reports";
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<Building> Buildings { get; set; }
    public virtual DbSet<Floor> Floors { get; set; }
    
    public virtual DbSet<RegisterType> RegisterTypes { get; set; }
    public virtual DbSet<RegisterFieldGroup> RegisterFieldGroups { get; set; }
    public virtual DbSet<RegisterField> RegisterFields { get; set; }
    
    public virtual DbSet<Report> Reports { get; set; }
    public virtual DbSet<ReportData> ReportDatas { get; set; }
    public virtual DbSet<ReportAttachment> ReportAttachments { get; set; }
    public virtual DbSet<ReportComment> ReportComments { get; set; }
    public virtual DbSet<Alert> Alerts { get; set; }
    
}