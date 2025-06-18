using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;

namespace Soditech.IntelPrev.Reports.Shared.RegisterTypes;

public record RegisterTypeResult
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public List<RegisterFieldResult> RegisterFields { get; set; } = default!;

    public List<RegisterFieldGroupResult> RegisterFieldGroups { get; set; } = default!;

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

public record CreateRegisterTypeCommand : IRequest<TResult<RegisterTypeResult>>
{
    public string Name { get; set; } = string.Empty;  // Type name (ex. SAVDHAS, Incident)
    public string Description { get; set; } = string.Empty; 
    public bool IsActive { get; set; }
    
    public List<CreateRegisterFieldCommand> RegisterFields { get; set; } = default!; // list of specifics fields
    
    public List<CreateRegisterFieldGroupCommand> RegisterFieldGroups { get; set; } = default!; // list of specifics field groups
}

public record UpdateRegisterTypeCommand : IRequest<TResult<RegisterTypeResult>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;  // Type name (ex. SAVDHAS, Incident)
    public string Description { get; set; } = string.Empty; 
    public bool IsActive { get; set; }
    
    public virtual List<UpdateRegisterFieldCommand> RegisterFields { get; set; } = default!; // list of specifics fields
    
    public List<UpdateRegisterFieldGroupCommand> RegisterFieldGroups { get; set; } = default!; // list of specifics field groups
}

public record DeleteRegisterTypeCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetRegisterTypeQuery : IRequest<TResult<RegisterTypeResult>>
{
    public Guid Id { get; set; }
}

public record GetRegisterTypesQuery : IRequest<TResult<IEnumerable<RegisterTypeResult>>>;

public record GetRegisterTypesCountQuery : IRequest<TResult<int>>;