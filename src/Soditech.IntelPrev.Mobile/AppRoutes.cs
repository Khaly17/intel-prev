namespace Soditech.IntelPrev.Mobile;

public static class AppRoutes
{
	public const string LoginPage = "//LoginView";
    public const string MainViewPage = "//MainView";
	public const string ProfilePage = "//ProfileView";
	public const string ForgotPasswordPage = "//ForgotPasswordView";
	public const string ChangePasswordPage = "//ChangePasswordView";
	public const string PinVerificationPage = "//PinVerificationView";
	

	#region Prevention
	public const string PreventionsPage = $"{MainViewPage}/PreventionsView";
	public const string DefinitionPreventionPage = $"{PreventionsPage}/DefinitionPreventionView";
	public const string PreventionStatisticInfosPage = $"{PreventionsPage}/PreventionStatisticInfosView";
	public const string PreventionStatisticsPage = $"{PreventionStatisticInfosPage}/PreventionStatisticsView";

	public const string PreventionActorsPage = $"{PreventionsPage}/PreventionActorsView";
	public const string EventsPage = $"{PreventionsPage}/EventsView";
	public const string EventDetailPage = $"{EventsPage}/EventDetailView";
	public const string EquipmentDetailPage = "EquipmentDetailView";
	public const string MaterialsPage = $"{PreventionsPage}/MaterialsView";
	public const string MaterialDetailPage = $"{MaterialsPage}/MaterialDetailView";
	public const string EquipmentLocationPage = $"{MaterialDetailPage}/EquipmentLocationView";

	public const string MedicalContactsPage = $"{PreventionsPage}/MedicalContactsView";

	// Buildings
	public const string PreventionBuildingsPage = $"{PreventionsPage}/PreventionBuildingsView";

	#endregion

	public const string FireSafetyStatisticsPage = $"{PreventionStatisticInfosPage}/FireSafetyStatisticsView";
	public const string AlertsStatisticsPage = $"{PreventionStatisticInfosPage}/AlertsStatisticsView";
	public const string TutosPage = $"{MainViewPage}/TutosView";
	public const string VideoPlayerPage = $"{TutosPage}/VideoPlayerView";
	public const string DocumentLegauxPage = $"{MainViewPage}/DocumentLegauxView";
	public const string DUERPPage = $"{MainViewPage}/DUERPView";
	public const string RegisterListPage = $"{MainViewPage}/RegisterListView";
	public const string CreateReportPage = $"{RegisterListPage}/CreateReportView";
	public const string ReportCreatedPage = $"{MainViewPage}/ReportCreatedView";

	#region Alerts
	public const string AlertsListPage = $"{MainViewPage}/AlertsListView";
	public const string AlertBuildingSelectionPage = $"{AlertsListPage}/BuildingSelectionView";
	public const string AlertFloorSelectionPage = $"{AlertBuildingSelectionPage}/FloorSelectionView";
	public const string AlertSummaryPage = $"{AlertBuildingSelectionPage}/SummaryView";
	public const string AlertCreatedPage = "AlertCreatedView";
	#endregion

	public const string DevPage = "DevView";
	public const string PdfViewer = "PdfViewerView";

	public const string SensibilisationOverviewPage = $"{PreventionsPage}/SensibilisationOverviewView";
	public const string RiskPreventionTipsPage = $"{MainViewPage}/RiskPreventionTipsView";

	public const string GpsMainPage = "GpsMainView";
	public const string MaterialLocationPage = "MaterialLocationView";

	#region Pro-Prev
	public const string ProPrevHomePage = $"{MainViewPage}/ProPrevHomeView";
	public const string RiskAnalysisProtocolPage = $"{ProPrevHomePage}/RiskAnalysisProtocolView";
	public const string AnalysisToolsPage = $"{ProPrevHomePage}/AnalysisToolsView";
	public const string ActionTrackingPage = $"{ProPrevHomePage}/ActionTrackingView";
	public const string SiteVisitePage = $"{ProPrevHomePage}/SiteVisiteView";
	public const string AgendaCSEPage = $"{ProPrevHomePage}/AgendaCSEView";
	public const string EPIControlPage = $"{ProPrevHomePage}/EPIControlView";
	public const string FDSPage = $"{ProPrevHomePage}/FDSView";
	public const string FirstAidPage = $"{ProPrevHomePage}/FirstAidView";
	public const string TrainingPage = $"{ProPrevHomePage}/TrainingView";
	public const string SecurityMeetingPage = $"{ProPrevHomePage}/SecurityMeetingView";
	public const string MyLibraryPage = $"{ProPrevHomePage}/MyLibraryView";

	#endregion

	#region FireSafety
	public const string FireSafetyListPage = $"{MainViewPage}/FireSafetyListView";
	public const string FireSafetyPage = $"{FireSafetyListPage}/FireSafetyView";
	public const string KnowYourCompanyPage = $"{FireSafetyListPage}/KnowYourCompanyView";
	public const string FireServicePage = $"{FireSafetyListPage}/FireServiceView";
	public const string FireInstructionsPage = $"{FireSafetyListPage}/FireInstructionsView";
	public const string FireEquipmentPage = $"{FireSafetyListPage}/FireEquipmentView";
	public const string EvacuationPage = $"{FireSafetyListPage}/EvacuationView";
	#endregion

	#region Gps

	public const string EquipmentTypeSelectionPage = "EquipmentTypeSelectionView";
	public const string EquipmentLocationTrackerPage = "EquipmentLocationTrackerView";

	#endregion

	#region Settings
	public const string SettingsPage = $"{MainViewPage}/SettingsView";
	#endregion
}
