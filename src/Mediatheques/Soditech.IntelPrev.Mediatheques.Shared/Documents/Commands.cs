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

    private byte[] _blobFile = Array.Empty<byte>();
   
    public IReadOnlyList<string> BlobFile => _blobFile;

    public string Path { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Type { get; set; } = DocumentType.Tutos.ToString();
    public string FileType { get; set; } = FileTypeEnum.Video.ToString();
    public bool IsDownloadable { get; set; }
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = default!;
}


public record GetDocumentsQuery : IRequest<TResult<IEnumerable<DocumentResult>>>;

public record GetDocumentsByTypeQuery : IRequest<TResult<IEnumerable<DocumentResult>>>
{
    public string Type { get; set; } = DocumentType.Tutos.ToString();
}

public record GetDocumentQuery(Guid Id) : IRequest<TResult<DocumentResult>>;
public record GetDocumentBytesQuery(string Path) : IRequest<TResult<byte[]>>;


public record GetDocumentsCountQuery : IRequest<TResult<int>>;

public record CreateFileFormByteCommand : IRequest<Result>
{
    public string FileName { get; set; } = string.Empty;
    private byte[] _blobFile = Array.Empty<byte>();

       public IReadOnlyList<string> BlobFile => _blobFile;


}

public record CreateDocumentCommand : IRequest<TResult<DocumentResult>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    private byte[] _blobFile = Array.Empty<byte>();
    public IReadOnlyList<string> BlobFile => _blobFile;

    public string Extension { get; set; } = string.Empty;

    [AllowedValues("DUERP", "InternalRules", "Tutos")]
    public string Type { get; set; } = DocumentType.Tutos.ToString();

    /// <summary>
    /// It will be used to display the file on the UI
    /// </summary>
    [AllowedValues("Video", "Image", "Document", "Pdf", "Audio", "Other")]
    public string FileType { get; set; } = FileTypeEnum.Video.ToString();

    public bool IsDownloadable { get; set; }
}


public record UpdateDocumentCommand : IRequest<TResult<DocumentResult>>
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    
    
    public string? Path { get; set; } = string.Empty;

    [AllowedValues("DUERP", "InternalRules", "Tutos")]
    public string Type { get; set; } = DocumentType.Tutos.ToString();

    /// <summary>
    /// It will be used to display the file on the UI
    /// </summary>
    [AllowedValues("Video", "Image", "Document", "Pdf", "Audio", "Other")]
    public string FileType { get; set; } = FileTypeEnum.Video.ToString();

    public bool? IsDownloadable { get; set; }
}


public record DeleteDocumentCommand(Guid Id) : IRequest<Result>;
