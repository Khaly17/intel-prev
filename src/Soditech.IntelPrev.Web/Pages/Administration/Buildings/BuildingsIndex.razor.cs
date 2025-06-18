using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Syncfusion.Blazor.Grids;

namespace Soditech.IntelPrev.Web.Pages.Administration.Buildings;

public partial class BuildingsIndex: ComponentBase
{
    public IList<BuildingResult> BuildingList { get; set; } = [];
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }


    private static List<GridColumn> Columns =>
    [
        new() { Field =  nameof(BuildingResult.Name), HeaderText = "Nom" },
            new() { Field = nameof(BuildingResult.Address), HeaderText = "Adresse" },
            new() { Field = nameof(BuildingResult.Description), HeaderText = "Description" }
    ];
    // toolbar items
    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private const string BuildingsCacheKey = "Buildings";
    private BuildingResult SelectedBuilding { get; set; } = default!;

    [Inject] private ILogger<BuildingsIndex> Logger { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        await LoadBuildingsAsync();
    }

    private async Task LoadBuildingsAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(BuildingsCacheKey);

        if (exists)
        {
            BuildingList = (IList<BuildingResult>)cachedValue;
        }
        else
        {
            await LoadBuildingsFromApiAsync();
            CacheService.Set(BuildingsCacheKey, BuildingList);
        }
        IsLoading = false;
    }
    private async Task LoadBuildingsFromApiAsync()
    {

        IsLoading = true;
        var buildingsResult = await ProxyService.GetAsync<IList<BuildingResult>>(PreventionRoutes.Buildings.GetAll);

        if (buildingsResult.IsSuccess)
        {
            BuildingList = buildingsResult.Value;
        }

        IsLoading = false;
    }
    private void DeleteBuildingTrigger(BuildingResult building)
    {
        SelectedBuilding = building;
        _isDeleteModalVisible = true;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;

    // add method 
    private void AddBuiling()
    {
        Navigation.NavigateTo("/buildings/add");
    }

    private async Task DeleteBuildingAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(PreventionRoutes.Buildings.Delete.Replace("{id:guid}", SelectedBuilding.Id.ToString()));

            if (result.IsSuccess)
            {
                BuildingList = BuildingList.Where(n => n.Id != SelectedBuilding.Id).ToList();
                ShowAlert("Building deleted successfully.");
            }
            else
            {
                ShowAlert("Failed to delete User.", "danger");
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting User: {Message}", ex.Message);
            ShowAlert("An error occurred while deleting the User. Please try again.", "danger");
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

    private void GoToEdit(BuildingResult building)
    {

        Navigation.NavigateTo($"buildings/edit/{building.Id}");

    }
}
