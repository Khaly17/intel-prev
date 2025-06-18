using System;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Prevensions.Shared.FireSecuritySetting;

public class UpdateDefinitionContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateKnownMyEnterpriseContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateFireSecurityServiceContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateFireConsignsContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}
public class UpdateFireMaterialsContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateEvacuationCaseContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}



public class DefinitionContentQuery : IRequest<TResult<FireSecuritySettingContentResult>>;

public class KnownMyEnterpriseContentQuery : IRequest<TResult<FireSecuritySettingContentResult>>;

public class FireSecurityServiceContentQuery : IRequest<TResult<FireSecuritySettingContentResult>>;

public class FireConsignsContentQuery : IRequest<TResult<FireSecuritySettingContentResult>>;

public class FireMaterialsContentQuery : IRequest<TResult<FireSecuritySettingContentResult>>;

public class EvacuationCaseContentQuery : IRequest<TResult<FireSecuritySettingContentResult>>;

public record FireSecuritySettingContentResult
{
    public Guid Id { get; init; }
    public bool IsDeleted { get; init; }
    
    public string Content { get; set; } = string.Empty;
   
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
