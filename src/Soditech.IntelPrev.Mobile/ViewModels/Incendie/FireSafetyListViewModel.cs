using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Syncfusion.Maui.Carousel;

namespace Soditech.IntelPrev.Mobile.ViewModels.Incendie;

public class FireSafetyListViewModel : MauiViewModel
{
    public ICommand NavigateWhatIsFireSafetyCommand => new AsyncRelayCommand(NavigateFireSafetyAsync);
    public ICommand NavigateKnowYourCompanyCommand => new AsyncRelayCommand(NavigateKnowYourCompanyAsync);
    public ICommand NavigateFireServiceCommand => new AsyncRelayCommand(NavigateFireServiceAsync);
    public ICommand NavigateFireInstructionsCommand => new AsyncRelayCommand(NavigateFireInstructionsAsync);
    public ICommand NavigateFireEquipmentCommand => new AsyncRelayCommand(NavigateFireEquipmentAsync);
    public ICommand NavigateEvacuationCommand => new AsyncRelayCommand(NavigateEvacuationAsync);
    public ICommand GoToDevViewCommand => new RelayCommand(GoToDevViewAsync);
    // Add carousel item changed command
    public ICommand CarouselItemChangedCommand => new RelayCommand<CurrentItemChangedEventArgs>(OnCarouselItemChanged);

    private List<SfCarouselItem> _carouselItems;
    public List<SfCarouselItem> CarouselItems
    {
        get => _carouselItems;
        set => SetProperty(ref _carouselItems, value);
    }

    public FireSafetyListViewModel()
    {
        CarouselItems =
        [
            new SfCarouselItem() { ImageName = "securite1.png" },
            new SfCarouselItem() { ImageName = "securite2.png" },
            new SfCarouselItem() { ImageName = "securite3.png" }
        ];
    }

    // Add handler for carousel item changed event
    private void OnCarouselItemChanged(CurrentItemChangedEventArgs args)
    {
        SfCarouselItem? previousItem = args.PreviousItem as SfCarouselItem;
        SfCarouselItem? currentItem _ = args.CurrentItem as SfCarouselItem;
        // Add additional logic here if needed
    }

    private async Task GoToDevViewAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.DevPage}"));
    }

    private async Task NavigateFireSafetyAsync()
    {
        await Shell.Current.GoToAsync(AppRoutes.FireSafetyPage);
    }

    private async Task NavigateKnowYourCompanyAsync()
    {
        await Shell.Current.GoToAsync(AppRoutes.KnowYourCompanyPage);
    }

    private async Task NavigateFireServiceAsync()
    {
        await Shell.Current.GoToAsync(AppRoutes.FireServicePage);
    }

    private async Task NavigateFireInstructionsAsync()
    {
        await Shell.Current.GoToAsync(AppRoutes.FireInstructionsPage);
    }

    private async Task NavigateFireEquipmentAsync()
    {
        await Shell.Current.GoToAsync(AppRoutes.FireEquipmentPage);
    }

    private async Task NavigateEvacuationAsync()
    {
        await Shell.Current.GoToAsync(AppRoutes.EvacuationPage);
    }
}
