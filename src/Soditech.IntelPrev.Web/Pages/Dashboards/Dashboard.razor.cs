using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Alerts;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;
using Soditech.IntelPrev.Reports.Shared.Reports;
using Soditech.IntelPrev.Web.Models;
using Soditech.IntelPrev.Web.Services.Extensions;
using Syncfusion.Blazor.Charts;
using System.Web;
using Soditech.IntelPrev.Web.Services.Helper;

namespace Soditech.IntelPrev.Web.Pages.Dashboards;

public partial class Dashboard : IAsyncDisposable
{
    [Inject]
    private ILogger<Dashboard> Logger { get; set; } = default!;
    private IList<ReportResult> reports = [];
    private IEnumerable<CountReportsGroupedByRegisterResult> ReportsGroupedByRegisterResult = [];
    private IEnumerable<CountAlertsGroupedByTypeResult> AlertsGroupedByTypeResult = [];

    private bool _isLoadingReports {  get; set; }
    private bool _isLoadingAlerts {  get; set; }
    private bool _isLoadingAssigment {  get; set; }
    private bool _isLoadingNotConformAssigment {  get; set; }

    private DateFilter DateFilter { get; set; } = default!;
    private HubConnection _hubConnection;
    private IList<string> messages = [];
    [Inject] private IConfiguration Configuration { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {

        DateFilter = new DateFilter();

        #region get date filter from query string

        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var query = HttpUtility.ParseQueryString(uri.Query);

        var startDate = query["startDate"];
        var endDate = query["endDate"];

        if (DateTime.TryParse(startDate, out var dateFilterStartDate))
        {
            DateFilter.StartDate = dateFilterStartDate;
        }

        if (DateTime.TryParse(endDate, out var dateFilterEndDate))
        {
            DateFilter.EndDate = dateFilterEndDate;
        }

        #endregion
        var baseUrl = Configuration["HostApi:BaseUri"];
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(baseUrl + "/notifications")
            .WithAutomaticReconnect()
            .Build();
            
        _hubConnection.On<string>("ReceiveNotification", (message) =>
        {

            _= LoadCharts();
            Logger.LogInformation($"Notification reçue : {message}");
            StateHasChanged();
        });

        try
        {
            await _hubConnection.StartAsync();
            Logger.LogInformation("Connexion SignalR établie.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors de la connexion au hub SignalR.");
        }
        await LoadCharts();
    }

    private async Task LoadCharts()
    {
        //TODO: set boolean here
        _isLoadingAlerts = true;
        _isLoadingAssigment = true;
        _isLoadingNotConformAssigment = true;
        _isLoadingReports = true;

        StateHasChanged();

        var tasks = new List<Task>
            {
                //TODO: call methods here
                GetReportsGroupedByRegisterAsync(),
                GetAlertsGroupedByTypeResult(),
            };

        await Task.WhenAll(tasks);

        StateHasChanged();
    }
    private async Task SetCurrentWeek()
    {
        var today = DateTime.Today;
        var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        DateFilter.StartDate = today.AddDays(-1 * diff).Date;
        DateFilter.EndDate = DateFilter.StartDate.AddDays(6).Date;
        await LoadCharts();
    }

    private async Task SetCurrentMonth()
    {
        var today = DateTime.Today;
        DateFilter.StartDate = new DateTime(today.Year, today.Month, 1);
        DateFilter.EndDate = DateFilter.StartDate.AddMonths(1).AddDays(-1);
        await LoadCharts();
    }

    private async Task GetAlertsGroupedByTypeResult()
    {
        _isLoadingReports = true;
        var path = ReportRoutes
                    .Alerts
                    .GetCountAlertsGroupedByType
                    .AddQueryParameters(DateFilter);
        var result = await ProxyService.GetAsync<IList<CountAlertsGroupedByTypeResult>>(path);
        if (result.IsSuccess)
        {
            AlertsGroupedByTypeResult = result.Value;
        }
        else
        {
            Logger.LogError($"Error occured while fetch alert list: {result.Error.Code}: {result.Error.Message}");
        }

        _isLoadingReports = false;

        StateHasChanged();


    }

    private async Task GetReportsGroupedByRegisterAsync()
    {
        _isLoadingReports = true;
        var path = ReportRoutes
                    .Reports
                    .GetCountReportsGroupedByRegister
                    .AddQueryParameters(DateFilter);

        var result = await ProxyService.GetAsync<IList<CountReportsGroupedByRegisterResult>>(path);

        if (result.IsSuccess)
        {
            ReportsGroupedByRegisterResult = result.Value;
        }
        else
        {
            Logger.LogError($"Error occured while fetch report list: {result.Error.Code}: {result.Error.Message}");
        }

        _isLoadingReports = false;

        StateHasChanged();
    }
    private async Task GetAlertsAsync()
    {
        //TODO: Not yet implemented
        await Task.CompletedTask;
    }

    private async Task GetAssigmentsAsync()
    {
        //TODO: Not yet implemented
        await Task.CompletedTask;
    }

    private async Task GetNotConformAssigmentsAsync()
    {
        //TODO: Not yet implemented
        await Task.CompletedTask;
    }

    private string NavigateToReportsDetails()
    {
        return "/report-details".AddQueryParameters(DateFilter);
    }

    private string NavigateToAlertsDetails()
    {
        return "/alert-details".AddQueryParameters(DateFilter);
    }
    private string NavigateToAssignmentsDetails()
    {
        return "/assigment-details".AddQueryParameters(DateFilter);
    }

    private string NavigateToNotConformAssignmentsDetails()
    {
        return "/not-conform-assigment-details".AddQueryParameters(DateFilter);
    }

    private async Task PrintCharts()
    {
        await Js.PrintCharts("Dashboard", "charts");
    }

    private async Task DownloadCharts()
    {
        //use DateFilter to get the file name
        var fileName = $"Dashboard_{DateFilter.StartDate:yyyy-MM-dd}_{DateFilter.EndDate:yyyy-MM-dd}";
        await Js.DownloadCharts("charts", fileName);
    }
    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}