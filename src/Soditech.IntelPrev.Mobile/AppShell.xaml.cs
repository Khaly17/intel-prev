using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Soditech.IntelPrev.Mobile.Views;
using Soditech.IntelPrev.Mobile.Views.Alerts;
using Soditech.IntelPrev.Mobile.Views.Gps;
using Soditech.IntelPrev.Mobile.Views.Incendie;
using Soditech.IntelPrev.Mobile.Views.Preventions;
using Soditech.IntelPrev.Mobile.Views.Preventions.Actors;
using Soditech.IntelPrev.Mobile.Views.Preventions.Buildings;
using Soditech.IntelPrev.Mobile.Views.Preventions.Events;
using Soditech.IntelPrev.Mobile.Views.Preventions.Materials;
using Soditech.IntelPrev.Mobile.Views.Preventions.MedicalContacts;
using Soditech.IntelPrev.Mobile.Views.Preventions.PreventionStatisticInfos;
using Soditech.IntelPrev.Mobile.Views.ProPrev;
using Soditech.IntelPrev.Mobile.Views.Reports;
using Soditech.IntelPrev.Mobile.Views.Tutoriels;
using Soditech.IntelPrev.Mobile.Views.Sensibilisation;
using Soditech.IntelPrev.Mobile.Views.Settings;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Helpers;
using Soditech.IntelPrev.Mobile.Views.DocumentLegaux;

namespace Soditech.IntelPrev.Mobile;

public partial class AppShell : Shell
{
	private readonly ILogger<AppShell> _logger = DependencyResolver.GetRequiredService<ILogger<AppShell>>();
	public AppShell()
	{
		try
		{
			InitializeComponent();
			RegisterRoutes();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error during AppShell initialization");
			// Don't rethrow - let the shell initialize with default values
		}
	}

	//BackCommand
	public ICommand BackCommand;


	/// <summary>
	/// Registers all navigation routes in the application
	/// </summary>
	private void RegisterRoutes()
	{
		Routing.RegisterRoute(AppRoutes.LoginPage, typeof(Views.Account.LoginView));
		Routing.RegisterRoute(AppRoutes.ChangePasswordPage, typeof(Views.Account.ChangePasswordView));
		Routing.RegisterRoute(AppRoutes.ForgotPasswordPage, typeof(Views.Account.ForgotPasswordView));
		Routing.RegisterRoute(AppRoutes.PinVerificationPage, typeof(Views.Account.PinVerificationView));
		Routing.RegisterRoute(AppRoutes.ProfilePage, typeof(Views.Account.ProfileView));
		Routing.RegisterRoute(AppRoutes.MainViewPage, typeof(MainView));

		Routing.RegisterRoute(AppRoutes.PreventionsPage, typeof(PreventionsView)); // level 1

		Routing.RegisterRoute(AppRoutes.DefinitionPreventionPage, typeof(DefinitionPreventionView)); // level 2
		Routing.RegisterRoute(AppRoutes.PreventionStatisticInfosPage, typeof(PreventionStatisticInfosView)); // level 2

		Routing.RegisterRoute(AppRoutes.PreventionStatisticsPage, typeof(PreventionStatisticsView)); // level 3
		Routing.RegisterRoute(AppRoutes.FireSafetyStatisticsPage, typeof(FireSafetyStatisticsView)); // level 3
		Routing.RegisterRoute(AppRoutes.AlertsStatisticsPage, typeof(AlertsStatisticsView)); // level 3

		Routing.RegisterRoute(AppRoutes.PreventionActorsPage, typeof(PreventionActorsView));
		Routing.RegisterRoute(AppRoutes.EventsPage, typeof(EventsView));
		Routing.RegisterRoute(AppRoutes.EventDetailPage, typeof(EventDetailView)); // level 3

		Routing.RegisterRoute(AppRoutes.MaterialsPage, typeof(MaterialsView));
		Routing.RegisterRoute(AppRoutes.EquipmentDetailPage, typeof(EquipmentDetailView));
		Routing.RegisterRoute(AppRoutes.EquipmentLocationPage, typeof(EquipmentLocationUpdateView));

		Routing.RegisterRoute(nameof(DocumentLegauxView), typeof(DocumentLegauxView));
		Routing.RegisterRoute(AppRoutes.TutosPage, typeof(TutosView)); // level 1
		Routing.RegisterRoute(AppRoutes.VideoPlayerPage, typeof(VideoPlayerView)); // level 2
		Routing.RegisterRoute(AppRoutes.DevPage, typeof(DevView)); // level 1

		Routing.RegisterRoute(AppRoutes.RegisterListPage, typeof(RegisterListView)); // level 1
		Routing.RegisterRoute(AppRoutes.CreateReportPage, typeof(CreateReportView)); // level 2
		Routing.RegisterRoute(AppRoutes.ReportCreatedPage, typeof(ReportCreatedView)); // level 2

		Routing.RegisterRoute(AppRoutes.SensibilisationOverviewPage, typeof(SensibilisationOverviewView));
		Routing.RegisterRoute(AppRoutes.RiskPreventionTipsPage, typeof(RiskPreventionTipsView));
		Routing.RegisterRoute(AppRoutes.PdfViewer, typeof(PdfViewerView));

		Routing.RegisterRoute(AppRoutes.AlertsListPage, typeof(AlertsListView)); // level 1
		Routing.RegisterRoute(AppRoutes.AlertBuildingSelectionPage, typeof(BuildingSelectionView)); // level 2
		Routing.RegisterRoute(AppRoutes.AlertSummaryPage, typeof(AlertSummaryCreationView)); // level 4
		Routing.RegisterRoute(AppRoutes.AlertCreatedPage, typeof(AlertCreatedView)); // level 5

		// Add GPS feature routes
		Routing.RegisterRoute(AppRoutes.GpsMainPage, typeof(GpsMainView));
		Routing.RegisterRoute(AppRoutes.MaterialLocationPage, typeof(MaterialLocationView));
		Routing.RegisterRoute(nameof(EquipmentTypeSelectionView), typeof(EquipmentTypeSelectionView));
		Routing.RegisterRoute(AppRoutes.EquipmentLocationTrackerPage, typeof(EquipmentLocationTrackerView));

		//PROPREV
		Routing.RegisterRoute(AppRoutes.ProPrevHomePage, typeof(ProPrevHomeView)); // level
		Routing.RegisterRoute(AppRoutes.RiskAnalysisProtocolPage, typeof(RiskAnalysisProtocolView)); // level
		Routing.RegisterRoute(AppRoutes.AnalysisToolsPage, typeof(AnalysisToolsView)); // level
		Routing.RegisterRoute(AppRoutes.ActionTrackingPage, typeof(ActionTrackingView)); // level
		Routing.RegisterRoute(AppRoutes.SiteVisitePage, typeof(SiteVisiteView)); // level
		Routing.RegisterRoute(AppRoutes.AgendaCSEPage, typeof(AgendaCSEView)); // level
		Routing.RegisterRoute(AppRoutes.EPIControlPage, typeof(EPIControlView)); // level
		Routing.RegisterRoute(AppRoutes.FDSPage, typeof(FDSView)); // level
		Routing.RegisterRoute(AppRoutes.FirstAidPage, typeof(FirstAidView)); // level
		Routing.RegisterRoute(AppRoutes.TrainingPage, typeof(TrainingView)); // level
		Routing.RegisterRoute(AppRoutes.SecurityMeetingPage, typeof(SecurityMeetingView)); // level
		Routing.RegisterRoute(AppRoutes.MyLibraryPage, typeof(MyLibraryView)); // level

		// incendie
		Routing.RegisterRoute(AppRoutes.FireSafetyListPage, typeof(FireSafetyListView));
		Routing.RegisterRoute(nameof(FireSafetyView), typeof(FireSafetyView));
		Routing.RegisterRoute(nameof(KnowYourCompanyView), typeof(KnowYourCompanyView));
		Routing.RegisterRoute(nameof(FireInstructionsView), typeof(FireInstructionsView));
		Routing.RegisterRoute(nameof(FireEquipmentView), typeof(FireEquipmentView));
		Routing.RegisterRoute(nameof(EvacuationView), typeof(EvacuationView));

		//prevention
		Routing.RegisterRoute(AppRoutes.PreventionBuildingsPage, typeof(PreventionBuildingsView));
		Routing.RegisterRoute(AppRoutes.MedicalContactsPage, typeof(MedicalContactsView));

		// Settings
		Routing.RegisterRoute(AppRoutes.SettingsPage, typeof(SettingsView));
	}

	/// <summary>
	/// Applies visual configuration for modern UI appearance
	/// </summary>
	private void ApplyVisualConfiguration()
	{
		// Set background colors and visual elements for a consistent look
		this.BackgroundColor = ThemeHelper.Primary;
		this.FlyoutBackgroundColor = ThemeHelper.PureWhite;
		// Apply status bar styling for a modern look
		this.Behaviors.Add(new StatusBarBehavior
		{
			StatusBarColor = ThemeHelper.Primary,
			StatusBarStyle = StatusBarStyle.LightContent
		});

		// Additional visual configurations can be added here
	}



	private static async Task HandleLogoutAsync(object? sender, EventArgs eventArgs)
	{
		if (sender is not Button button) return;
		// Visual feedback when logout is clicked
		await button.ScaleTo(0.95, 100);
		await button.ScaleTo(1.0, 100);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = OnPageAppearedAsync(); 
    }

    private async Task OnPageAppearedAsync()
    {
        ApplyVisualConfiguration();

        var permissions = new List<Func<Task<PermissionStatus>>>
    {
        Permissions.RequestAsync<Permissions.LocationWhenInUse>,
        Permissions.RequestAsync<Permissions.PostNotifications>,
    };

        foreach (var permissionRequest in permissions)
        {
            await permissionRequest();
        }
    }
}

// User model class to represent the user returned from the API
public record UserInfo
{
	public string Id { get; set; } = string.Empty;
	public string TenantId { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public List<string> Roles { get; set; } = default!;
}