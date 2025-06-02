using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Preventions.Persistence.Models;

public class Statistic : EntityBase
{
    public string Category { get; set; } = string.Empty;
    public float Value { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    
    public Guid CampaignId { get; set; }
    public virtual Campaign Campaign { get; set; } = default!;
    
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