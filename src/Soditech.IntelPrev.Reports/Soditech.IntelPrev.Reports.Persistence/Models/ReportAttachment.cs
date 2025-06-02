using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Reports.Persistence.Models;

public class ReportAttachment : EntityBase
{
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; //(e.g., "image/png", "application/pdf").
    public string FilePath { get; set; } = string.Empty;
    
    public Guid ReportId { get; set; }
    public virtual Report Report { get; set; } = default!;
    
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