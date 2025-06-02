using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Preventions.Shared.StaticContents;

public record StaticContentResult
{
    public Guid Id { get; init; }
    public bool IsDeleted { get; init; }
    
    public string Key { get; set; } = string.Empty; // e.g., "WhatIsPrevention", "Sensibilisation", "Risques", "Consignes"
    public string Title { get; set; } = string.Empty;  // e.g., "C'est quoi la prévention"
    public string Content { get; set; } = string.Empty;  // e.g., the detailed explanation
    
    public string? CreatorFullName { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    
    public string? DeleterFullName { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public string? UpdaterFullName { get; set; }
    public Guid? UpdaterId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public record CreateStaticContentCommand : IRequest<TResult<StaticContentResult>>
{
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public record UpdateStaticContentCommand : IRequest<TResult<StaticContentResult>>
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public record DeleteStaticContentCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetStaticContentQuery(Guid Id) : IRequest<TResult<StaticContentResult>>;

public record GetStaticContentsCountQuery : IRequest<TResult<int>>;

public record GetStaticContentsQuery : IRequest<TResult<IEnumerable<StaticContentResult>>>;

public record GetStaticContentByKeyQuery(string Key) : IRequest<TResult<StaticContentResult>>;

public record GetStaticContentByKeysQuery(IEnumerable<string> Keys) : IRequest<TResult<IEnumerable<StaticContentResult>>>;

