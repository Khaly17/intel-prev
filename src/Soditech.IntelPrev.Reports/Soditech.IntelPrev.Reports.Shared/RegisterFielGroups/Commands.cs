using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.Shared.RegisterFieldGroups;

public record RegisterFieldGroupResult
{
	public string Name { get; set; } = string.Empty;  // Field name (ex. "Identité", "Détails")
	public string Description { get; set; } = string.Empty; // group description
	public int DisplayOrder { get; set; } // order of display

	public Guid RegisterTypeId { get; set; } // reference to the register type
	public string RegisterTypeName { get; set; } = default!;

	public List<RegisterFieldResult> RegisterFields { get; set; } = default!;

	public Guid Id { get; set; }
	public bool IsDeleted { get; set; }

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

public record CreateRegisterFieldGroupCommand : IRequest<TResult<RegisterFieldGroupResult>>
{
	public string Name { get; set; } = string.Empty;  // Field name (ex. "Identité", "Détails")
	public string Description { get; set; } = string.Empty; // group description
	public int DisplayOrder { get; set; } // order of display
	public Guid RegisterTypeId { get; set; }
	public List<CreateRegisterFieldCommand> RegisterFields { get; set; } = default!; // list of specifics fields

}

public record CreateReportFieldGroupCommand : IRequest<TResult<RegisterFieldGroupResult>>
{
	public string Name { get; set; } = string.Empty;  // Field name (ex. "Identité", "Détails")
	public string Description { get; set; } = string.Empty; // group description
	public int DisplayOrder { get; set; } // order of display
	public Guid RegisterTypeId { get; set; }
	public IEnumerable<CreateReportFieldCommand> CreateReportFieldsCommand { get; set; } = [];
}

public record UpdateRegisterFieldGroupCommand : IRequest<TResult<RegisterFieldGroupResult>>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;  // Field name (ex. "Identité", "Détails")
	public string Description { get; set; } = string.Empty; // group description
	public int DisplayOrder { get; set; } // order of display
	public Guid RegisterTypeId { get; set; }

	public virtual List<UpdateRegisterFieldCommand> RegisterFields { get; set; } = default!; // list of specifics fields
}

public record DeleteRegisterFieldGroupCommand : IRequest<Result>
{
	public Guid Id { get; set; }
}

public record GetRegisterFieldGroupQuery(Guid Id) : IRequest<TResult<RegisterFieldGroupResult>>;
public record GetRegisterFieldGroupsCountQuery : IRequest<TResult<int>>;
public record GetRegisterFieldGroupsQuery : IRequest<TResult<IEnumerable<RegisterFieldGroupResult>>>;