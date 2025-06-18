using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Soditech.IntelPrev.Mobile.ViewModels.Reports;
using Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;

namespace Soditech.IntelPrev.Mobile.Helpers;

/// <summary>
/// Helper class to process fields with additional metadata for UI organization
/// without modifying the core CreateReportField class
/// </summary>
public static class ReportFieldMetadataHelper
{
	// These are common prefixes that indicate critical fields
	private static readonly string[] CriticalPrefixes =
	[
		"id", "reference", "date", "who", "what", "where", "when",
		"incident", "type", "urgence", "urgency", "priority", "location", "site"
	];

	// Fields with these names are considered critical regardless of prefix
	private static readonly string[] CriticalNames =
	[
		"description", "title", "titre", "summary", "résumé", "cause", "impact"
	];

	/// <summary>
	/// Organizes fields into sections based on implicit metadata
	/// </summary>
	public static List<SectionViewModel> OrganizeFieldsIntoSections(IEnumerable<object> fields)
	{
		var sections = new List<SectionViewModel>();
		var fieldCommands = fields.OfType<CreateReportFieldCommand>().ToList();
		var fieldGroups = fields.OfType<CreateReportFieldGroupCommand>().ToList();

		// If there are fewer than 5 fields, just create a single section
		if (fieldCommands.Count < 5 && !fieldGroups.Any())
		{
			sections.Add(new SectionViewModel
			{
				Title = "Informations",
				Fields = new ObservableCollection<object>(fieldCommands)
			});
			return sections;
		}

		// Group fields by their implicit purpose
		var criticalFields = new List<CreateReportFieldCommand>();
		var locationFields = new List<CreateReportFieldCommand>();
		var descriptionFields = new List<CreateReportFieldCommand>();
		var otherFields = new List<CreateReportFieldCommand>();

		foreach (var field in fieldCommands)
		{
			// Check if it's a critical field
			if (IsCriticalField(field))
			{
				criticalFields.Add(field);
			}
			// Check if it's location-related
			else if (IsLocationField(field))
			{
				locationFields.Add(field);
			}
			// Check if it's description-related
			else if (IsDescriptionField(field))
			{
				descriptionFields.Add(field);
			}
			else
			{
				otherFields.Add(field);
			}
		}

		// Create sections based on the categorization
		if (criticalFields.Any())
		{
			sections.Add(new SectionViewModel
			{
				Title = "Informations essentielles",
				Fields = new ObservableCollection<object>(criticalFields),
				IsCritical = true
			});
		}

		if (locationFields.Any())
		{
			sections.Add(new SectionViewModel
			{
				Title = "Lieu",
				Fields = new ObservableCollection<object>(locationFields)
			});
		}

		if (descriptionFields.Any())
		{
			sections.Add(new SectionViewModel
			{
				Title = "Détails",
				Fields = new ObservableCollection<object>(descriptionFields)
			});
		}

		if (otherFields.Any())
		{
			sections.Add(new SectionViewModel
			{
				Title = "Informations supplémentaires",
				Fields = new ObservableCollection<object>(otherFields)
			});
		}

		// Add field groups as separate sections
		foreach (var group in fieldGroups)
		{
			sections.Add(new SectionViewModel
			{
				Title = group.Name,
				Fields = new ObservableCollection<object>(group.CreateReportFieldsCommand)
			});
		}

		return sections;
	}

	/// <summary>
	/// Determines if a field is a critical field based on heuristics
	/// </summary>
	public static bool IsCriticalField(CreateReportFieldCommand field)
	{
		if (field == null) return false;

		// Check if field is marked as required
		if (field.IsRequired)
		{
			return true;
		}

		var nameLower = field.Name.ToLowerInvariant();

		// Check for critical name matches
		if (CriticalNames.Any(cn => nameLower.Contains(cn)))
		{
			return true;
		}

		// Check for critical prefixes
		return CriticalPrefixes.Any(cp => nameLower.StartsWith(cp));
	}

	/// <summary>
	/// Determines if a field is related to location
	/// </summary>
	public static bool IsLocationField(CreateReportFieldCommand field)
	{
		if (field == null) return false;

		var nameLower = field.Name.ToLowerInvariant();
		return nameLower.Contains("location") ||
		       nameLower.Contains("lieu") ||
		       nameLower.Contains("adresse") ||
		       nameLower.Contains("site") ||
		       nameLower.Contains("emplacement") ||
		       nameLower.Contains("bâtiment") ||
		       nameLower.Contains("batiment") ||
		       nameLower.Contains("étage") ||
		       nameLower.Contains("etage") ||
		       nameLower.Contains("zone") ||
		       nameLower.Contains("salle") ||
		       nameLower.Contains("bureau");
	}

	/// <summary>
	/// Determines if a field is related to description
	/// </summary>
	public static bool IsDescriptionField(CreateReportFieldCommand field)
	{
		if (field == null) return false;

		var nameLower = field.Name.ToLowerInvariant();
		return nameLower.Contains("description") ||
		       nameLower.Contains("detail") ||
		       nameLower.Contains("détail") ||
		       nameLower.Contains("comment") ||
		       nameLower.Contains("observation") ||
		       nameLower.Contains("note");
	}
}