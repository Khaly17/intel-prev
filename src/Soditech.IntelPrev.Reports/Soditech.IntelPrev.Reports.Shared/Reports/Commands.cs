using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Shared.Enums;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.Shared.Reports;


public record ReportResult
{
	public string Title { get; set; } = string.Empty; // Register title
	public string Description { get; set; } = string.Empty; // General description 
	public string Status { get; set; } = string.Empty;

	public Guid RegisterTypeId { get; set; } // reference to the register
	public string RegisterTypeTitle { get; set; } = default!;

	public List<ReportDataResult> ReportDatas { get; set; } = default!;

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

public record CreateReportCommand : IRequest<TResult<ReportResult>>
{
	public string Title { get; set; } = string.Empty; // Register title
	public string Description { get; set; } = string.Empty; // General description 
	public string Status { get; set; } = string.Empty;
	public Guid RegisterTypeId { get; set; } // reference to the register
	public List<CreateReportDataCommand> ReportDatas { get; set; } = default!;
	public IEnumerable<object> GetSortedFieldsAndGroups { get; set; } = default!;

	// Constructeur sans paramètres
	public CreateReportCommand() { }


	public void UpdateCreateReportDataCommand()
	{
		ReportDatas = [];
		foreach (var item in GetSortedFieldsAndGroups)
		{
			switch (item)
			{
				case CreateReportFieldCommand field:
					ReportDatas.Add(new CreateReportDataCommand
					{
						FieldId = field.RegisterFieldId,
						Value = field.Value?.ToString() ?? string.Empty
					});
					break;
				case CreateReportFieldGroupCommand groupCommand:
					{
						foreach (var field in groupCommand.CreateReportFieldsCommand)
						{
							ReportDatas.Add(new CreateReportDataCommand
							{
								FieldId = field.RegisterFieldId,
								Value = field.Value?.ToString() ?? string.Empty
							});
						}

						break;
					}
				
				default:
					throw new ArgumentException($"Type de champ non supporté dans le switch : {item?.GetType().Name ?? "null"}",
						nameof(item)
					);
			}
		}
	}

	public CreateReportCommand(RegisterTypeResult registerType)
	{

		Title = registerType.Name;
		Description = registerType.Description;
		Status = ReportStatus.Open.ToString();
		RegisterTypeId = registerType.Id;
		ReportDatas = [];

		// Add all fields to the report
		foreach (var field in registerType.RegisterFields)
		{
			ReportDatas.Add(new CreateReportDataCommand
			{
				Value = string.Empty,
				//ReportId = report.Id,
				FieldId = field.Id
			});
		}

		var groups = registerType.RegisterFieldGroups
			.OrderBy(g => g.DisplayOrder)
			.Select(f => new CreateReportFieldGroupCommand()
			{
				Name = f.Name,
				Description = f.Description,
				DisplayOrder = f.DisplayOrder,
				RegisterTypeId = f.RegisterTypeId,
				CreateReportFieldsCommand = f.RegisterFields
					.Select(rf => new CreateReportFieldCommand()
					{
						Name = rf.Name,
						Description = rf.Description,
						FieldType = rf.FieldType,
						DisplayOrder = rf.DisplayOrder,
						IsRequired = rf.IsRequired,
						RegisterTypeId = rf.RegisterTypeId,
						RegisterFieldGroupId = rf.RegisterFieldGroupId,
						RegisterFieldId = rf.Id,
						Value = GetDefaultValue(rf.FieldType)
					})
					.OrderBy(rf => rf.DisplayOrder)
					.ToList()
			})
			.ToList();

		var fieldsWithoutGroup = registerType.RegisterFields
			.Where(f => f.RegisterFieldGroupId == null)
			.Select(f => new CreateReportFieldCommand()
			{
				Name = f.Name,
				Description = f.Description,
				FieldType = f.FieldType,
				DisplayOrder = f.DisplayOrder,
				IsRequired = f.IsRequired,
				RegisterTypeId = f.RegisterTypeId,
				RegisterFieldGroupId = f.RegisterFieldGroupId,
				RegisterFieldId = f.Id,
				Value = GetDefaultValue(f.FieldType)
			})
			.OrderBy(f => f.DisplayOrder)
			.ToList();

		// merge groups and fields without group
		GetSortedFieldsAndGroups = groups.Cast<object>()
			.Concat(fieldsWithoutGroup)
			.OrderBy(GetDisplayOrder)
			.ToList();
	}

	private static int GetDisplayOrder(object item)
	{
		return item switch
		{
			CreateReportFieldGroupCommand group => group.DisplayOrder,
			CreateReportFieldCommand field => field.DisplayOrder,
			_ => 0
		};
	}

	private static object GetDefaultValue(string fieldType)
	{
		if (fieldType == FieldType.Boolean.ToString())
		{
			return false;
		}

		if (fieldType == FieldType.Date.ToString())
		{
			return DateTimeOffset.UtcNow;
		}

		if (fieldType == FieldType.Number.ToString())
		{
			return 0;
		}

		return string.Empty;
	}
}

public record UpdateReportCommand : IRequest<TResult<ReportResult>>
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty; // Register title
	public string Description { get; set; } = string.Empty; // General description 
	public string Status { get; set; } = string.Empty;

	public Guid RegisterTypeId { get; set; } // reference to the register

	public List<UpdateReportDataCommand> ReportDatas { get; set; } = default!;
}

public record DeleteReportCommand : IRequest<Result>
{
	public Guid Id { get; set; }
}

public record GetReportQuery : IRequest<TResult<ReportResult>>
{
	public Guid Id { get; set; }
}

public record GetReportsCountQuery : IRequest<TResult<int>>;

public record GetReportsQuery : IRequest<TResult<IEnumerable<ReportResult>>>;

#region get reports by register type

public record GetReportsGroupedByRegisterQuery : IRequest<TResult<IEnumerable<ReportResult>>>
{
	public DateTimeOffset StartDate { get; set; }
	public DateTimeOffset EndDate { get; set; }
}

public record GetCountReportsGroupedByRegisterQuery : IRequest<TResult<IEnumerable<CountReportsGroupedByRegisterResult>>>
{
	public DateTimeOffset StartDate { get; set; }
	public DateTimeOffset EndDate { get; set; }
}

public record CountReportsGroupedByRegisterResult
{
	public required string RegisterTypeName { get; set; }
	public int Count { get; set; }

}

#endregion