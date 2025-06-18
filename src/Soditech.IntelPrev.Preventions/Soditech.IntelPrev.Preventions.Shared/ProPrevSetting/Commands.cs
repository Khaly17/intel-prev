using System;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Prevensions.Shared.ProPrevSetting;

public class UpdateRiskAnalysisProtocolContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateAnalysisToolsContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateActionsOrganizerContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateCseAgendaContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}
public class UpdateDataSheetContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class UpdateEpiControlContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}
public class HealthFormationContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}
public class MyLibraryContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}
public class SecurityQuarterContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}

public class SitesVisitContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}
public class FirstAidKitContentCommand : IRequest<Result>
{
    public required string Content { get; set; }
}



public class RiskAnalysisProtocolContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class AnalysisToolsContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class ActionsOrganizerContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class CseAgendaContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class DataSheetContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class EpiControlContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class HealthFormationContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class MyLibraryContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class SecurityQuarterContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class SitesVisitContentQuery : IRequest<TResult<ProPrevContentResult>>;

public class FirstAidKitContentQuery : IRequest<TResult<ProPrevContentResult>>;

public record ProPrevContentResult
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
