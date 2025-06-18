using System;
using MediatR;
using Soditech.IntelPrev.Mediatheques.Shared.Enums;

namespace Soditech.IntelPrev.Mediatheques.Shared.Documents;

public record DocumentCreatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public string Path { get; set; } = string.Empty;
    public DocumentType Type { get; set; }

    /// <summary>
    /// It will be used to display the file on the UI
    /// </summary>
    public FileType FileType { get; set; }

    public bool IsDownloadable { get; set; }

    public Guid TenantId { get; set; }
}

public record DocumentUpdatedEvent : INotification
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public string? UpdaterFullName { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    
    public string Path { get; set; } = string.Empty;
    public DocumentType Type { get; set; }

    /// <summary>
    /// It will be used to display the file on the UI
    /// </summary>
    public FileType FileType { get; set; }

    public bool IsDownloadable { get; set; }

}

public record DocumentDeletedEvent(Guid Id) : INotification;