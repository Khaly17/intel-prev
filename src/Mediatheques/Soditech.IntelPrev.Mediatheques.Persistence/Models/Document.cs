using Sensor6ty.Domain;
using Soditech.IntelPrev.Mediatheques.Shared.Enums;

namespace Soditech.IntelPrev.Mediatheques.Persistence.Models;

public class Document : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public DocumentType Type { get; set; }

    /// <summary>
    /// It will be used to display the file on the UI
    /// </summary>
    public FileType FileType { get; set; }

    public string Description { get; set; } = string.Empty;
    public bool IsDownloadable { get; set; }

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