using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Shared.Enums;

namespace Soditech.IntelPrev.Mediatheques.Shared.Documents;

public record DocumentResult
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? CreatorFullName { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public string? DeleterFullName { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public string? UpdaterFullName { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    private ICollection<byte> _blobFile = [];
    public ICollection<byte> BlobFile
    {
        get => _blobFile;
        set => _blobFile = value;
    }

    public string Path { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Type { get; set; } = nameof(DocumentType.Tutos);
    public string FileType { get; set; } = nameof(FileTypeEnum.Video);
    public bool IsDownloadable { get; set; }
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
}


public record GetDocumentsQuery : IRequest<TResult<IEnumerable<DocumentResult>>>;

public record GetDocumentsByTypeQuery : IRequest<TResult<IEnumerable<DocumentResult>>>
{
    public string Type { get; set; } = nameof(DocumentType.Tutos);
}

public record GetDocumentQuery(Guid Id) : IRequest<TResult<DocumentResult>>;
public record GetDocumentBytesQuery(string Path) : IRequest<TResult<byte[]>>;


public record GetDocumentsCountQuery : IRequest<TResult<int>>;

public record CreateFileFormByteCommand : IRequest<Result>
{
    public string FileName { get; set; } = string.Empty;
    private ICollection<byte> _blobFile = [];
    public ICollection<byte> BlobFile
    {
        get => _blobFile; 
        set => _blobFile = value;
    }

}

public record CreateDocumentCommand : IRequest<TResult<DocumentResult>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    private ICollection<byte> _blobFile = [];
    public ICollection<byte> BlobFile
    {
        get => _blobFile;
        set => _blobFile = value;
    }


    public string Extension { get; set; } = string.Empty;

    [AllowedValues("DUERP", "InternalRules", "Tutos")]
    public string Type { get; set; } = nameof(DocumentType.Tutos);

    /// <summary>
    /// It will be used to display the file on the UI
    /// </summary>
    [AllowedValues("Video", "Image", "Document", "Pdf", "Audio", "Other")]
    public string FileType { get; set; } = nameof(FileTypeEnum.Video);

    public bool IsDownloadable { get; set; }
}


public record UpdateDocumentCommand : IRequest<TResult<DocumentResult>>
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    
    
    public string? Path { get; set; } = string.Empty;

    [AllowedValues("DUERP", "InternalRules", "Tutos")]
    public string Type { get; set; } = nameof(DocumentType.Tutos);

    /// <summary>
    /// It will be used to display the file on the UI
    /// </summary>
    [AllowedValues("Video", "Image", "Document", "Pdf", "Audio", "Other")]
    public string FileType { get; set; } = nameof(FileTypeEnum.Video);

    public bool? IsDownloadable { get; set; }
}


public record DeleteDocumentCommand(Guid Id) : IRequest<Result>;
