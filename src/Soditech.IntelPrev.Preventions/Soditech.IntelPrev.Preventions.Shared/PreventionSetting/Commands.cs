using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Preventions.Shared.PreventionSetting;

public class UpdateSensibilisationContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateDefinitionContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}


public class SensibilisationContentQuery : IRequest<TResult<PreventionContentResult>>;

public class DefinitionContentQuery : IRequest<TResult<PreventionContentResult>>;

public record PreventionContentResult
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
