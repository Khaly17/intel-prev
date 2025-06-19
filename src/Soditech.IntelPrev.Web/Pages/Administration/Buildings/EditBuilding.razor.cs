using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;

namespace Soditech.IntelPrev.Web.Pages.Administration.Buildings;

public partial class EditBuilding
{
    [Parameter]
    public string BuildingId { get; set; } = string.Empty;
    public string Title { get; set; } = "Modification du batiment";
    private BuildingResult Building { get; set; } = new();

    private string? _successMessage;

    private string? _errorMessage;
    private bool _isLoading = false;
    [Inject] private ILogger<EditBuilding> Logger { get; set; } = default!;
    private const string BuildingsCacheKey = "Buildings";
    private string GetBuildingCacheKey() => $"Building_{BuildingId}";

    protected override async Task OnInitializedAsync()
    {
        await LoadCampaignsAsync();
    }

    private async Task LoadCampaignsAsync()
    {
        _isLoading = true;
        var cacheKey = GetBuildingCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            Building = (BuildingResult)cachedValue;
        }
        else
        {
            await LoadBuildingFromApiAsync();
            CacheService.Set(cacheKey, Building);
        }
        _isLoading = false;
    }
    private async Task LoadBuildingFromApiAsync()
    {
        try
        {
            _isLoading = true;
            var result = await ProxyService.GetAsync<BuildingResult>(PreventionRoutes.Buildings.GetById.Replace("{id:guid}", BuildingId));

            if (result.IsSuccess)
            {
                Building = result.Value;

            }
            else
            {
                _errorMessage = "Erreur de récupération des informations du batiment.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Erreur: {ex.Message}";
        }
        _isLoading = false;
    }

    private async Task UpdateBuildingAsync()
    {
        if (Building.Id == Guid.Empty)
        {
            _errorMessage = "L'ID du batiment est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<BuildingResult>(PreventionRoutes.Buildings.Update.Replace("{id:guid}", Building.Id.ToString()), Building);
        if (updateResult.IsSuccess)
        {
            _successMessage = "Batiment mis à jour avec succès.";
            _errorMessage = null;
            CacheService.Set(GetBuildingCacheKey(), Building);
            CacheService.Set(BuildingsCacheKey, null);
            Navigation.NavigateTo("/buildings");
        }
        else
        {
            _errorMessage = "Erreur lors de la mise à jour des informations du batiment.";
        }
    }

}
