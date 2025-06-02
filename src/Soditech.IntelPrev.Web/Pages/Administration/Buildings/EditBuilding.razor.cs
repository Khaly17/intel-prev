using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared.Buildings;
using Soditech.IntelPrev.Preventions.Shared;

namespace Soditech.IntelPrev.Web.Pages.Administration.Buildings;

public partial class EditBuilding
{
    [Parameter]
    public string buildingId { get; set; } = string.Empty;
    public string title { get; set; } = "Modification du batiment";
    private BuildingResult building { get; set; } = new();

    private string? successMessage;

    private string? errorMessage;
    private bool IsLoading = false;
    [Inject] private ILogger<EditBuilding> Logger { get; set; } = default!;
    private const string BuildingsCacheKey = "Buildings";
    private string GetBuildingCacheKey() => $"Building_{buildingId}";

    protected override async Task OnInitializedAsync()
    {
        await LoadCampaignsAsync();
    }

    private async Task LoadCampaignsAsync()
    {
        IsLoading = true;
        var cacheKey = GetBuildingCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            building = (BuildingResult)cachedValue;
        }
        else
        {
            await LoadBuildingFromApiAsync();
            CacheService.Set(cacheKey, building);
        }
        IsLoading = false;
    }
    private async Task LoadBuildingFromApiAsync()
    {
        try
        {
            IsLoading = true;
            var result = await ProxyService.GetAsync<BuildingResult>(PreventionRoutes.Buildings.GetById.Replace("{id:guid}", buildingId));

            if (result.IsSuccess)
            {
                building = result.Value;

            }
            else
            {
                errorMessage = "Erreur de récupération des informations du batiment.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur: {ex.Message}";
        }
        IsLoading = false;
    }

    private async Task UpdateBuilding()
    {
        if (building.Id == Guid.Empty)
        {
            errorMessage = "L'ID du batiment est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<BuildingResult>(PreventionRoutes.Buildings.Update.Replace("{id:guid}", building.Id.ToString()), building);
        if (updateResult.IsSuccess)
        {
            successMessage = "Batiment mis à jour avec succès.";
            errorMessage = null;
            CacheService.Set(GetBuildingCacheKey(), building);
            CacheService.Set(BuildingsCacheKey, null);
            Navigation.NavigateTo("/buildings");
        }
        else
        {
            errorMessage = "Erreur lors de la mise à jour des informations du batiment.";
        }
    }

}
