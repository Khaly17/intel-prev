using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Localization;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Syncfusion.Maui.Carousel;
using Soditech.IntelPrev.Mobile.Views.DocumentLegaux;
using Soditech.IntelPrev.Mobile.Views.Incendie;
using Soditech.IntelPrev.Mobile.Views.Preventions;

namespace Soditech.IntelPrev.Mobile.ViewModels;

public class MainViewModel : MauiViewModel
{
	// Déclaration initiale des éléments du carrousel avec l'extension .png correcte
	private List<SfCarouselItem>? _carouselItems = [
		new SfCarouselItem() { ImageName = "sensibilisation1.png" },
		new SfCarouselItem() { ImageName = "sensibilisation2.png" },
		new SfCarouselItem() { ImageName = "sensibilisation3.png" },
		new SfCarouselItem() { ImageName = "sensibilisation4.png" },
		new SfCarouselItem() { ImageName = "sensibilisation5.png" }
	];

	public List<SfCarouselItem> CarouselItems
	{
		get => _carouselItems;
		set => SetProperty(ref _carouselItems, value);
	}

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

	



	public ICommand CallPoliceCommand => new RelayCommand(CallPoliceAsync);
	public ICommand CallFirefighterCommand => new RelayCommand(CallFirefighterAsync);
	public ICommand PreventionsViewCommand => new AsyncRelayCommand(PreventionsViewAsync);
	public ICommand GoToDocumentCommand => new AsyncRelayCommand(GoToDocumentAsync);
	public ICommand GoToTutosCommand => new AsyncRelayCommand(GoToTutosAsync);
	public ICommand GoToCreateReportCommand => new AsyncRelayCommand(GoToCreateReportPageAsync);
	public ICommand GoToAlertsListPageCommand => new AsyncRelayCommand(GoToAlertsListPageAsync);
	public ICommand GoToProPrevHomePageCommand => new AsyncRelayCommand(GoToProPrevHomePageAsync);
	// go to devview
	public ICommand GoToDevViewCommand => new AsyncRelayCommand(GoToDevViewAsync);

	// Go to GPS page
	public ICommand GoToGpsPageCommand => new AsyncRelayCommand(GoToGpsPageAsync);
	public ICommand GoToIncendieViewCommand => new AsyncRelayCommand(GoToIncendieViewAsync);
	// Go to Settings page
	public ICommand GoToSettingsCommand => new AsyncRelayCommand(GoToSettingsPageAsync);

	// on currentItemchanged on carousel
	private async Task PreventionsViewAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState($"{nameof(PreventionsView)}"));
	}
	private void CallPoliceAsync()
	{
		if (PhoneDialer.Default.IsSupported)
		{
			PhoneDialer.Default.Open("778508294");
		}
		else
		{
			UserDialogs.Instance.Alert(L.Localize("CallApiIsNotSupportedMessage"), L.Localize("CallApiIsNotSupported"));
		}
	}

	private void CallFirefighterAsync()
	{
		if (PhoneDialer.Default.IsSupported)
		{
			PhoneDialer.Default.Open("768640428");
		}
		else
		{
			UserDialogs.Instance.Alert(L.Localize("CallApiIsNotSupportedMessage"), L.Localize("CallApiIsNotSupported"));
		}
	}

	private async Task GoToDocumentAsync()
	{
		await Shell.Current.GoToAsync($"{nameof(DocumentLegauxView)}");
	}

	private async Task GoToTutosAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.TutosPage}"));
	}

	private async Task GoToCreateReportPageAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.RegisterListPage}"));
	}

	private async Task GoToAlertsListPageAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.AlertsListPage));
	}

	private async Task GoToProPrevHomePageAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.ProPrevHomePage));
	}

	private async Task GoToDevViewAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.DevPage}"));
	}

	private async Task GoToGpsPageAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.GpsMainPage));
	}

	private async Task GoToIncendieViewAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(nameof(FireSafetyListView)));
	}

	private async Task GoToSettingsPageAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.SettingsPage));
	}
}