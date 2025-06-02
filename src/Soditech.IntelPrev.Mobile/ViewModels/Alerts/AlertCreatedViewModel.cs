using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Preventions.Shared.Floors;
using Soditech.IntelPrev.Preventions.Shared.Buildings;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Mobile.ViewModels.Alerts;

public class AlertCreatedViewModel : MauiViewModel
{

	private string _successMessage = "Votre alerte a été envoyée avec succès!";
	public string SuccessMessage
	{
		get => _successMessage;
		set => SetProperty(ref _successMessage, value);
	}

	private string _promptMessage = "Que souhaitez-vous faire?";
	public string PromptMessage
	{
		get => _promptMessage;
		set => SetProperty(ref _promptMessage, value);
	}

	private string _primaryActionText = "Effectuer une autre alerte";
	public string PrimaryActionText
	{
		get => _primaryActionText;
		set => SetProperty(ref _primaryActionText, value);
	}

	private string _secondaryActionText = "Retourner à l'Accueil";
	public string SecondaryActionText
	{
		get => _secondaryActionText;
		set => SetProperty(ref _secondaryActionText, value);
	}
	public ICommand ToGoHomeCommand => new AsyncRelayCommand(GoToHomePageAsync);

	public ICommand CreateNewAlertCommand => new AsyncRelayCommand(CreateNewAlertAsync);

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

	public override Task InitializeAsync()
	{
		// Additional initialization if needed
		return Task.CompletedTask;
	}

	//to go to home page
	private async Task GoToHomePageAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.MainViewPage));
	}

	// Navigate to create new alert screen
	private async Task CreateNewAlertAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.AlertsListPage));
	}


}