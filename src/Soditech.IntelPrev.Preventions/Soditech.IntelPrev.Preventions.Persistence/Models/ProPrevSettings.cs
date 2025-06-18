using System;
using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Preventions.Persistence.Models;

public class ProPrevSettings : EntityBase
{
    public string RiskAnalysisProtocolContent { get; set; } = string.Empty;
    public string AnalysisToolsContent { get; set; } = string.Empty;
    /// <summary>
    /// organize yourself and monitor your actions
    /// </summary>
    public string ActionsOrganizerContent { get; set; } = string.Empty;
    public string SitesVisitContent { get; set; } = string.Empty;
    /// <summary>
    /// CSE agenda
    /// </summary>
    public string CseAgendaContent { get; set; } = string.Empty;
    /// <summary>
    /// Security Quarter hour
    /// </summary>
    public string SecurityQuarterContent { get; set; } = string.Empty;
    /// <summary>
    /// EPI control
    /// </summary>
    public string EpiControlContent { get; set; } = string.Empty;
    /// <summary>
    /// Data and safety sheet
    /// </summary>
    public string DataSheetContent { get; set; } = string.Empty;
    /// <summary>
    /// First aid kit
    /// </summary>
    public string FirstAidKitContent { get; set; } = string.Empty;
    /// <summary>
    /// Health and work safety formation
    /// </summary>
    public string HealthFormationContent { get; set; } = string.Empty;
    public string MyLibraryContent { get; set; } = string.Empty;
    
    public bool IsDefault { get; set; }
    
    public Guid? TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; } = default!;
    
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