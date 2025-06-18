using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Syncfusion.Maui.Carousel;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions;

public class PreventionsViewModel : MauiViewModel
{
	public ICommand DefinitionPreventionCommand => new AsyncRelayCommand(DefinitionPreventionAsync);
	public ICommand PreventionStatisticInfosCommand => new AsyncRelayCommand(PreventionStatisticInfosAsync);
	public ICommand GoToDevViewCommand => new AsyncRelayCommand(GoToDevViewAsync);
	public ICommand GotoPreventionBuildings => new AsyncRelayCommand(GotoPreventionBuildingsAsync);
	public ICommand NavigateSensibilisationOverviewCommand => new AsyncRelayCommand(NavigateSensibilisationOverviewAsync);
	public ICommand NavigateRiskPreventionCommand => new AsyncRelayCommand(NavigateRiskPreventionAsync);
	public ICommand NavigatePreventionActorsCommand => new AsyncRelayCommand(NavigatePreventionActorsAsync);
	public ICommand NavigateEventsCommand => new AsyncRelayCommand(NavigateEventsAsync);
	public ICommand GoToMaterialsCommand => new AsyncRelayCommand(GoToMaterialsAsync);
	public ICommand GotoMedicalContactsCommand => new AsyncRelayCommand(GotoMedicalContactsAsync);
	// Add carousel item changed command
	public ICommand CarouselItemChangedCommand => new RelayCommand<CurrentItemChangedEventArgs>(OnCarouselItemChanged);

	private List<SfCarouselItem> _carouselItems;
	public List<SfCarouselItem> CarouselItems
	{
		get => _carouselItems;
		set => SetProperty(ref _carouselItems, value);
	}

	public PreventionsViewModel()
	{
		CarouselItems =
		[
			new SfCarouselItem() { ImageName = "sensib1.png" },
			new SfCarouselItem() { ImageName = "sensib2.png" },
			new SfCarouselItem() { ImageName = "sensib3.png" }
		];
	}

	// Add handler for carousel item changed event
	private void OnCarouselItemChanged(CurrentItemChangedEventArgs args)
	{
		// SfCarouselItem? previousItem = args.PreviousItem as SfCarouselItem;
		// SfCarouselItem? currentItem  = args.CurrentItem as SfCarouselItem;
		// Add additional logic here if needed
	}

	private async Task GotoPreventionBuildingsAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.PreventionBuildingsPage));
	}
	private async Task DefinitionPreventionAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.DefinitionPreventionPage));
	}

	private async Task PreventionStatisticInfosAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.PreventionStatisticInfosPage));
	}
	private async Task GoToDevViewAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.DevPage}"));
	}

	private async Task NavigateSensibilisationOverviewAsync()
	{
		await Shell.Current.GoToAsync(AppRoutes.SensibilisationOverviewPage);
	}

	private async Task NavigateRiskPreventionAsync()
	{
		await Shell.Current.GoToAsync(AppRoutes.RiskPreventionTipsPage);
	}

	private async Task NavigatePreventionActorsAsync()
	{
		await Shell.Current.GoToAsync(AppRoutes.PreventionActorsPage);
	}

	private async Task NavigateEventsAsync()
	{
		await Shell.Current.GoToAsync(AppRoutes.EventsPage);
	}

	private async Task GoToMaterialsAsync()
	{
		await Shell.Current.GoToAsync(AppRoutes.MaterialsPage);
	}

	private async Task GotoMedicalContactsAsync()
	{
		await Shell.Current.GoToAsync(AppRoutes.MedicalContactsPage);
	}
}
