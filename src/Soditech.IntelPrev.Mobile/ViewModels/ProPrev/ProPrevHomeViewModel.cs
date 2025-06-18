using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Syncfusion.Maui.Carousel;

namespace Soditech.IntelPrev.Mobile.ViewModels.ProPrev;

class ProPrevHomeViewModel : MauiViewModel
{
    public ICommand GoToDevViewCommand => new AsyncRelayCommand(GoToDevViewAsync);
    public ICommand GoToRiskAnalysisProtocolCommand => new AsyncRelayCommand(GoToRiskAnalysisProtocolPageAsync);
    public ICommand GoToAnalysisToolsCommand => new AsyncRelayCommand(GoToAnalysisToolsPageAsync);
    public ICommand GoToActionTrackingCommand => new AsyncRelayCommand(GoToActionTrackingPageAsync);
    public ICommand GoToSiteVisiteCommand => new AsyncRelayCommand(GoToSiteVisitePageAsync);
    public ICommand GoToAgendaCSECommand => new AsyncRelayCommand(GoToAgendaCSEPageAsync);
    public ICommand GoToEPIControlCommand => new AsyncRelayCommand(GoToEPIControlPageAsync);
    public ICommand GoToFDSCommand => new AsyncRelayCommand(GoToFDSPageAsync);
    public ICommand GoToFirstAidCommand => new AsyncRelayCommand(GoToFirstAidPageAsync);
    public ICommand GoToTrainingCommand => new AsyncRelayCommand(GoToTrainingPageAsync);
    public ICommand GoToSecurityMeetingCommand => new AsyncRelayCommand(GoToSecurityMeetingPageAsync);
    public ICommand GoToMyLibraryCommand => new AsyncRelayCommand(GoToMyLibraryPageAsync);


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