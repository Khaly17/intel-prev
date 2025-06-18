using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Helpers;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Mobile.ViewModels.Reports;

public partial class CreateReportViewModel : MauiViewModel, IQueryAttributable
{
	private CreateReportCommand _createReport;
	public CreateReportCommand CreateReport
	{
		get => _createReport;
		set => SetProperty(ref _createReport, value);
	}

	private RegisterTypeResult _registerType = default!;
	private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());
	public ICommand NextPageCommand => new AsyncRelayCommand(GoToNextPage);
	public ICommand PreviousPageCommand => new AsyncRelayCommand(GoToPreviousPage);

	// Progress indicator
	[ObservableProperty]
	private float _progress = 0.33f;

	// Sections for grouping fields
	[ObservableProperty]
	private ObservableCollection<SectionViewModel> _sections = new();

	// Use a single instance for visible sections
	[ObservableProperty]
	private ObservableCollection<SectionViewModel> _visibleSections = new();

	// Validation messages
	[ObservableProperty]
	private string _validationMessage = string.Empty;

	[ObservableProperty] private bool _hasValidationErrors;

	// Current section index
	[ObservableProperty]
	private int _currentSectionIndex;

	// Pagination properties
	[ObservableProperty]
	private bool _canGoBack;

	[ObservableProperty]
	private string _nextButtonText = "Suivant";

	[ObservableProperty]
	private string _currentPageDisplay = "Section 1/1";

	// Step indicators
	[ObservableProperty]
	private string _step1Color = "Gray";

	[ObservableProperty]
	private string _step2Color = "#007bff";

	[ObservableProperty]
	private string _step3Color = "Gray";

	[ObservableProperty]
	private FontAttributes _step1FontAttributes = FontAttributes.None;

	[ObservableProperty]
	private FontAttributes _step2FontAttributes = FontAttributes.Bold;

	[ObservableProperty]
	private FontAttributes _step3FontAttributes = FontAttributes.None;

	/// <inheritdoc />
	public override async Task InitializeAsync()
	{
		IsBusy = true;
		var registerTypeResult = await _proxyClientService.GetAsync<RegisterTypeResult>(ReportRoutes.RegisterTypes
				.GetById.Replace("{id:guid}", _registerType.Id.ToString()));

		if (registerTypeResult.IsSuccess && registerTypeResult.Value != null)
		{
			_registerType = registerTypeResult.Value;

			// Initialize form with new report
			CreateReport = new CreateReportCommand(_registerType);

			// Organize fields into sections
			OrganizeFieldsIntoSections();
		}
		else
		{
			// If we already have a register type from the navigation parameters,
			// we can continue with that instead of showing an error
			if (_registerType != null && _registerType.Id != Guid.Empty)
			{
				// Initialize form with the existing report type data
				CreateReport = new CreateReportCommand(_registerType);

				// Organize fields into sections
				OrganizeFieldsIntoSections();
			}
			else
			{
				await Shell.Current.DisplayAlert("Error", "Failed to load report type", "OK");
			}
		}

		IsBusy = false;
	}

	private void OrganizeFieldsIntoSections()
	{
		Sections.Clear();

		// Get all fields from report
		var allFields = CreateReport.GetSortedFieldsAndGroups.ToList();

		// Use our helper to organize fields into sections based on their implicit purpose
		var organizedSections = ReportFieldMetadataHelper.OrganizeFieldsIntoSections(allFields);

		// Update the Sections collection
		foreach (var section in organizedSections)
		{
			Sections.Add(section);
		}

		_currentSectionIndex = 0;
		UpdateVisibleSection(); // Update the collection
		UpdateNavigationButtons();
	}

	private void UpdateVisibleSection()
	{
		VisibleSections.Clear();
		if (Sections != null && Sections.Count > 0 && _currentSectionIndex >= 0 && _currentSectionIndex < Sections.Count)
		{
			VisibleSections.Add(Sections[_currentSectionIndex]);
		}
	}

	private void UpdateNavigationButtons()
	{
		CanGoBack = _currentSectionIndex > 0;
		NextButtonText = IsLastSection() ? "Soumettre" : "Suivant";

		// Update progress based on sections completed
		Progress = (float)(_currentSectionIndex + 1) / Sections.Count;

		// Update step indicators based on progress
		if (Progress < 0.33f)
		{
			Step1Color = "Gray";
			Step2Color = "#007bff";
			Step3Color = "Gray";

			Step1FontAttributes = FontAttributes.None;
			Step2FontAttributes = FontAttributes.Bold;
			Step3FontAttributes = FontAttributes.None;
		}
		else if (Progress < 0.85f)
		{
			Step1Color = "Gray";
			Step2Color = "#007bff";
			Step3Color = "Gray";

			Step1FontAttributes = FontAttributes.None;
			Step2FontAttributes = FontAttributes.Bold;
			Step3FontAttributes = FontAttributes.None;
		}
		else
		{
			Step1Color = "Gray";
			Step2Color = "Gray";
			Step3Color = "#007bff";

			Step1FontAttributes = FontAttributes.None;
			Step2FontAttributes = FontAttributes.None;
			Step3FontAttributes = FontAttributes.Bold;
		}

		CurrentPageDisplay = $"Section {_currentSectionIndex + 1}/{Sections.Count}";
	}

	private bool IsLastSection() => _currentSectionIndex >= Sections.Count - 1;

	private bool ValidateCurrentSection()
	{
		// Reset validation state
		HasValidationErrors = false;
		ValidationMessage = string.Empty;

		if (_currentSectionIndex < 0 || _currentSectionIndex >= Sections.Count)
			return true;

		var currentSection = Sections[_currentSectionIndex];
		var missingFields = new List<string>();

		foreach (var field in currentSection.Fields)
		{
			if (field is CreateReportFieldCommand reportField && reportField.IsRequired)
			{
				// Check if the required field has a value
				bool isEmpty = reportField.Value == null ||
							   (reportField.Value is string strValue && string.IsNullOrWhiteSpace(strValue)) ||
							   (reportField.Value is DateTime dateValue && dateValue == default);

				if (isEmpty)
				{
					missingFields.Add(reportField.Name);
				}
			}
		}

		if (missingFields.Count > 0)
		{
			ValidationMessage = $"Veuillez compléter tous les champs obligatoires : {string.Join(", ", missingFields)}";
			HasValidationErrors = true;
			return false;
		}

		return true;
	}

	private async Task GoToNextPage()
	{
		if (!ValidateCurrentSection())
		{
			await Shell.Current.DisplayAlert("Champs requis", ValidationMessage, "OK");
			return;
		}

		if (!IsLastSection())
		{
			_currentSectionIndex++;
			UpdateVisibleSection();
			UpdateNavigationButtons();
		}
		else
		{
			if (ValidateAllSections())
			{
				// Show summary before creating report
				var proceed = await Shell.Current.DisplayAlert("Confirmer",
					"Souhaitez-vous soumettre ce rapport maintenant ?",
					"Oui", "Non");

				if (proceed)
				{
					await CreateReportAsync();
				}
			}
			else
			{
				await Shell.Current.DisplayAlert("Validation",
					"Certains champs obligatoires n'ont pas été complétés. Veuillez vérifier toutes les sections.",
					"OK");
			}
		}
	}

	private bool ValidateAllSections()
	{
		// Store the current index to restore it later
		var originalIndex = _currentSectionIndex;

		try
		{
			for (int i = 0; i < Sections.Count; i++)
			{
				_currentSectionIndex = i;
				if (!ValidateCurrentSection())
				{
					return false;
				}
			}
			return true;
		}
		finally
		{
			// Restore the original section index
			_currentSectionIndex = originalIndex;
			UpdateVisibleSection(); // Update the collection
			UpdateNavigationButtons();
		}
	}

	public async Task GoToPreviousPage()
	{
		if (_currentSectionIndex > 0)
		{
			_currentSectionIndex--;
			UpdateVisibleSection();
			UpdateNavigationButtons();
		}
		else
		{
			// return to the previous page using the navigation stack
			await Shell.Current.GoToAsync("..");
		}
	}

	private async Task CreateReportAsync()
	{
		CreateReport.UpdateCreateReportDataCommand();
		var reportResult = await _proxyClientService.PostAsync<ReportResult>(ReportRoutes.Reports.Create, CreateReport);

		if (reportResult.IsSuccess)
		{
			// Update step indicators for "Validate" step
			Step1Color = "Gray";
			Step2Color = "Gray";
			Step3Color = "#007bff";

			Step1FontAttributes = FontAttributes.None;
			Step2FontAttributes = FontAttributes.None;
			Step3FontAttributes = FontAttributes.Bold;

			await Shell.Current.GoToAsync(AppRoutes.ReportCreatedPage, new Dictionary<string, object>
				{
					{ "Report", reportResult.Value }
				});
		}
		else
		{
			await Shell.Current.DisplayAlert("Error", $"Failed to create report {reportResult.Error.Code}", "OK");
		}
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("Register", out var register))
		{
			_registerType = (RegisterTypeResult)register;
			_ = InitializeAsync();
		}
	}
}

// SectionViewModel class for compatibility with existing code
public partial class SectionViewModel : ObservableObject
{
	private string _title;
	public string Title
	{
		get => _title;
		set => SetProperty(ref _title, value);
	}

	private Collection<object> _fields = new();
	public Collection<object> Fields
	{
		get => _fields;
		set => SetProperty(ref _fields, value);
	}

	private bool _isCritical;
	public bool IsCritical
	{
		get => _isCritical;
		set => SetProperty(ref _isCritical, value);
	}
}
