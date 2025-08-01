using System;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Preventions.Persistence.Models;

public class StaticContent : EntityBase
{
    public string Key { get; set; } = string.Empty; // e.g., "WhatIsPrevention", "Sensibilisation", "Risques", "Consignes"
    public string Title { get; set; } = string.Empty;  // e.g., "C'est quoi la prévention"
    public string Content { get; set; } = string.Empty;  // e.g., the detailed explanation
    
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