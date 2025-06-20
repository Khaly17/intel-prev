using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Mobile.ViewModels.Reports;

public class ReportCreatedViewModel : MauiViewModel, IQueryAttributable
{
	private ReportResult _report;
	public ReportResult Report
	{
		get => _report;
		set => SetProperty(ref _report, value);
	}

	private string _successMessage = "Votre signalement a été enregistré avec succès.";
	public string SuccessMessage
	{
		get => _successMessage;
		set => SetProperty(ref _successMessage, value);
	}

	private string _promptMessage = "Merci pour votre contribution";
	public string PromptMessage
	{
		get => _promptMessage;
		set => SetProperty(ref _promptMessage, value);
	}

	private string _primaryActionText = "Créer un autre signalement";
	public string PrimaryActionText
	{
		get => _primaryActionText;
		set => SetProperty(ref _primaryActionText, value);
	}

	private string _secondaryActionText = "Retour à l'accueil";
	public string SecondaryActionText
	{
		get => _secondaryActionText;
		set => SetProperty(ref _secondaryActionText, value);
	}

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

	public static ICommand SecondaryButtonCommand => new AsyncRelayCommand(async () =>
		await Shell.Current.GoToAsync($"{AppRoutes.MainViewPage}"));

	public static ICommand PrimaryButtonCommand => new AsyncRelayCommand(async () =>
		await Shell.Current.GoToAsync($"{AppRoutes.RegisterListPage}"));

	public override Task InitializeAsync()
	{
		// Additional initialization if needed
		return Task.CompletedTask;
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("Report", out var report))
		{
			Report = (ReportResult)report;
		}
	}
}
