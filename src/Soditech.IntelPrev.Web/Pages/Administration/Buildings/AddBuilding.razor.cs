using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Prevensions.Shared.Floors;

namespace Soditech.IntelPrev.Web.Pages.Administration.Buildings;

public partial class AddBuilding
{
    public BuildingResult NewBuilding { get; set; } = new();
    public string Title { get; set; } = "Ajouter un batiment";
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    [Inject] private ILogger<AddBuilding> Logger { get; set; } = default!;
    private List<FloorResult> NewFloors { get; set; } = new();
    private FloorResult NewFloor { get; set; } = new();

    private async Task CreateBuildingAsync()
    {
        ErrorMessage = null;
        SuccessMessage = null;

        try
        {
            var result = await ProxyService.PostAsync<BuildingResult>(PreventionRoutes.Buildings.Create, NewBuilding);

            if (result.IsSuccess)
            {
                SuccessMessage = "Le batiment a été ajouté avec succès !";
                Navigation.NavigateTo("/buildings");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création du bâtiment";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Une erreur interne est survenue lors de la création du bâtiment.";
            Logger.LogError(ex, ErrorMessage);
        }
    }

}
