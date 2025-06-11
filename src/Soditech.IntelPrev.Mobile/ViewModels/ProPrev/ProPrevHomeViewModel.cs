using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Syncfusion.Maui.Carousel;

namespace Soditech.IntelPrev.Mobile.ViewModels.ProPrev;

class ProPrevHomeViewModel : MauiViewModel
{
    public ICommand GoToDevViewCommand => new RelayCommand(GoToDevViewAsync);
    public ICommand GoToRiskAnalysisProtocolCommand => new RelayCommand(GoToRiskAnalysisProtocolPageAsync);
    public ICommand GoToAnalysisToolsCommand => new RelayCommand(GoToAnalysisToolsPageAsync);
    public ICommand GoToActionTrackingCommand => new RelayCommand(GoToActionTrackingPageAsync);
    public ICommand GoToSiteVisiteCommand => new RelayCommand(GoToSiteVisitePageAsync);
    public ICommand GoToAgendaCSECommand => new RelayCommand(GoToAgendaCSEPageAsync);
    public ICommand GoToEPIControlCommand => new RelayCommand(GoToEPIControlPageAsync);
    public ICommand GoToFDSCommand => new RelayCommand(GoToFDSPageAsync);
    public ICommand GoToFirstAidCommand => new RelayCommand(GoToFirstAidPageAsync);
    public ICommand GoToTrainingCommand => new RelayCommand(GoToTrainingPageAsync);
    public ICommand GoToSecurityMeetingCommand => new RelayCommand(GoToSecurityMeetingPageAsync);
    public ICommand GoToMyLibraryCommand => new RelayCommand(GoToMyLibraryPageAsync);
    // Add carousel item changed command
    public ICommand CarouselItemChangedCommand => new RelayCommand<CurrentItemChangedEventArgs>(OnCarouselItemChanged);


    private List<SfCarouselItem> _carouselItems;
    public List<SfCarouselItem> CarouselItems
    {
        get => _carouselItems;
        set => SetProperty(ref _carouselItems, value);
    }

    public ProPrevHomeViewModel()
    {
        // Initialize carousel items
        CarouselItems =
        [
            new SfCarouselItem() { ImageName = "image1.png" },
            new SfCarouselItem() { ImageName = "image2.png" },
            new SfCarouselItem() { ImageName = "image3.png" },
            new SfCarouselItem() { ImageName = "image4.png" },
            new SfCarouselItem() { ImageName = "image5.png" }
        ];
    }

    // Add handler for carousel item changed event
    private Task OnCarouselItemChanged(CurrentItemChangedEventArgs args)
    {
        SfCarouselItem? previousItem = args.PreviousItem as SfCarouselItem;
        SfCarouselItem? currentItem _ = args.CurrentItem as SfCarouselItem;
        // Add additional logic here if needed
    }

    private async Task GoToRiskAnalysisProtocolPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.RiskAnalysisProtocolPage));
    }

    private async Task GoToAnalysisToolsPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.AnalysisToolsPage));
    }

    private async Task GoToActionTrackingPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.ActionTrackingPage}"));
    }
    private async Task GoToSiteVisitePageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.SiteVisitePage}"));
    }
    private async Task GoToAgendaCSEPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.AgendaCSEPage}"));
    }
    private async Task GoToEPIControlPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.EPIControlPage}"));
    }
    private async Task GoToFDSPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.FDSPage}"));
    }
    private async Task GoToFirstAidPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.FirstAidPage}"));
    }
    private async Task GoToTrainingPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.TrainingPage}"));
    }
    private async Task GoToSecurityMeetingPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.SecurityMeetingPage}"));
    }
    private async Task GoToMyLibraryPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.MyLibraryPage}"));
    }
    private async Task GoToDevViewAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.DevPage}"));
    }
}