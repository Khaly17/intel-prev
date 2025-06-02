using Sensor6ty.Domain;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Reports.Persistence.Models;

public class Report: EntityBase
{
    public string Title { get; set; } = string.Empty; // Register title
    public string DisplayName { get; set; } = string.Empty;  // Type name (ex. SAVDHAS, Incident)

    public string Description { get; set; }  = string.Empty; // General description 
    public ReportStatus Status { get; set; }
    
    public Guid RegisterTypeId { get; set; } // register type reference
    public virtual RegisterType RegisterType { get; set; } = default!;
    public virtual List<ReportData> ReportDatas { get; set; } = default!;
    
    
    public Guid TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = default!;
    
    public virtual User? Creator { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public virtual User? Deleter { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual User? Updater { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}