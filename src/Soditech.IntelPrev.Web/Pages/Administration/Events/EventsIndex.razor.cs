using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Events;
using Syncfusion.Blazor.Grids;

namespace Soditech.IntelPrev.Web.Pages.Administration.Events;

public partial class EventsIndex : ComponentBase
{
    public IList<EventResult> EventList { get; set; } = [];
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }

    private static List<GridColumn> Columns =>
    [
        new() { Field = nameof(EventResult.Name), HeaderText = "Nom" },
        new() { Field = nameof(EventResult.StartDate), HeaderText = "Date de début" },
        new() { Field = nameof(EventResult.EndDate), HeaderText = "Date de fin" },
        new() { Field = nameof(EventResult.Location), HeaderText = "Lieu" },
        new() { Field = nameof(EventResult.Description), HeaderText = "Description" }
    ];
    // toolbar items
    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private EventResult SelectedEvent { get; set; } = default!;
    private const string EventsCacheKey = "Events";

    [Inject] private ILogger<EventsIndex> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadEventsFromApiAsync();
    }

    private async Task LoadEventsAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(EventsCacheKey);

        if (exists)
        {
            EventList = (IList<EventResult>)cachedValue;
        }
        else
        {
            await LoadEventsFromApiAsync();
            CacheService.Set(EventsCacheKey, EventList);
        }
        IsLoading = false;
    }
    private async Task LoadEventsFromApiAsync()
    {
        IsLoading = true;
        var eventsResult = await ProxyService.GetAsync<IList<EventResult>>(PreventionRoutes.Events.GetAll);

        if (eventsResult.IsSuccess)
        {
            EventList = eventsResult.Value;
        }

        IsLoading = false;
    }

    private void DeleteEventTrigger(EventResult eventItem)
    {
        SelectedEvent = eventItem;
        _isDeleteModalVisible = true;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;

    private void AddEvent()
    {
        Navigation.NavigateTo("/events/add");
    }

    private async Task DeleteEventAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(PreventionRoutes.Events.Delete.Replace("{id:guid}", SelectedEvent.Id.ToString()));

            if (result.IsSuccess)
            {
                EventList = EventList.Where(n => n.Id != SelectedEvent.Id).ToList();
                ShowAlert("Event deleted successfully.");
            }
            else
            {
                ShowAlert("Failed to delete event.", "danger");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting event: {Message}", ex.Message);
            ShowAlert("An error occurred while deleting the event. Please try again.", "danger");
        }
        finally
        {
            HideDeleteModal();
        }
    }

    private void ShowAlert(string message, string type = "success")
    {
        _alertMessage = message;
        _alertType = type;
        _isAlertVisible = true;
        Task.Delay(3000).ContinueWith(_ =>
        {
            _isAlertVisible = false;
            StateHasChanged();
        });
    }

    private void GoToEdit(EventResult eventItem)
    {
        Navigation.NavigateTo($"events/edit/{eventItem.Id}");
    }
}