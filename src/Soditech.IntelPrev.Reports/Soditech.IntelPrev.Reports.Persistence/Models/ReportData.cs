using System;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Reports.Persistence.Models;

public class ReportData: EntityBase
{
    public string Value { get; set; } = string.Empty; // Value of the property
    
    public Guid ReportId { get; set; } // reference to the register
    public virtual Report Report { get; set; } = null!;
    
    public Guid FieldId { get; set; } // reference to the field
    public virtual RegisterField Field { get; set; } = null!;
    
    public Guid TenantId { get; set; }
    public virtual Tenant Tenant { get; set; } = null!;
    
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